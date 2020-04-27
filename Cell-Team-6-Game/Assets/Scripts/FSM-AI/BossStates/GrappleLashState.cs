using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrappleLashState : FSMState
{
    private BossEnemy stateMachine;
    private float animtime;
    private float Range;
    private Animator chosenTent;
    private bool initialize = true;
    private bool behaviorComplete = false; //Set to True when the behavior is complete. This triggers transition back to Idle
    private Transform playerOriginalParent;

    private HealthScript tentHealth;

    private Collider2D tentacleHead;

    private PlayerController player;

    private bool success = false;

    public override void Act(Transform player, GameObject self) { }

    public GrappleLashState()
    {
        stateID = FSMStateID.GrappleLash;
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Dead Check
        if (self.GetComponent<BossEnemy>().coreHealthScript.currentHealth <= 0)
        {
            parentFSM.SetTransition(FSMTransitions.OutOfHealth);
        }

        //Completion Check
        else if (behaviorComplete)
        {
            parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        animtime = 1.0f;
        stateMachine = self.GetComponent<BossEnemy>();
        behaviorComplete = false;
        this.player = player.GetComponent<PlayerController>();
        Range = stateMachine.radRange.rad2;

        foreach (Animator tentacle in stateMachine.tentacles)
        {
            if (!tentacle) { continue; }
            if (initialize == true)
            {
                Range = Vector2.Distance(tentacle.transform.position, player.position);
                initialize = false;
            }
            else
            {
                if (Vector2.Distance(tentacle.transform.position, player.position) < Range)
                {
                    Range = Vector2.Distance(tentacle.transform.position, player.position);
                    chosenTent = tentacle;
                    Debug.Log(tentacle, tentacle);
                    Debug.Log(chosenTent, chosenTent);
                }
            }
        }

        if (!chosenTent) { behaviorComplete = true; return; }

        tentacleHead = stateMachine.tentacleColliders[chosenTent][7];
        tentHealth = chosenTent.GetComponent<HealthScript>();

        tentHealth.onCollidePlayer.AddListener(GrabPlayer);

        stateMachine.StartCoroutine(GrabAnim());
        tentHealth.invincible = false;

        foreach (CircleCollider2D bone in stateMachine.tentacleColliders[chosenTent])
        {
            bone.enabled = true;
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        if (chosenTent)
        {
            chosenTent.SetBool("IsGrapple", false);
            chosenTent.SetBool("IsHorizontal", false);
            tentHealth.onCollidePlayer.RemoveListener(GrabPlayer);
            tentHealth.invincible = true;

            foreach (CircleCollider2D bone in stateMachine.tentacleColliders[chosenTent])
            {
                bone.enabled = false;
            }
        }

        success = false;


        if (this.player)
        {
            if (this.player.frozen)
            {
                this.player.Freeze_Unfreeze();
            }
        }
    }

    public void GrabPlayer() {
        if (success) { return; }
        playerOriginalParent = player.transform.parent;
        player.transform.parent = tentacleHead.transform;
        player.Freeze_Unfreeze();
        success = true;
    }

    public void ReleasePlayer() {
        player.transform.parent = playerOriginalParent;
    }

    private IEnumerator GrabAnim() {
        chosenTent.SetBool("IsGrapple", true);
        yield return new WaitForSeconds(1);

        Debug.Log("success is " + success);
        if (success)
        {
            stateMachine.StartCoroutine(Throw());
            chosenTent.SetBool("IsGrapple", false);
        }
        else { behaviorComplete = true; }
        chosenTent.SetBool("IsGrapple", false);
    }

    private IEnumerator Throw() {
        chosenTent.SetBool("IsHorizontal", true);
        yield return new WaitForSeconds(1.14f);
        ReleasePlayer();
        player.Yeet(10, 1);
        player.Freeze_Unfreeze();
        behaviorComplete = true;
    }
}
