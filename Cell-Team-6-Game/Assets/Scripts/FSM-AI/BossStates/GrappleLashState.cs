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
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle
    private Transform playerOriginalParent;

    private HealthScript tentHealth;

    private Collider2D tentacleHead;

    private PlayerController player;

    private bool success = false;

    private UnityAction grabDel;

    public override void Act(Transform player, GameObject self)
    {
        animtime -= Time.deltaTime;
        if (animtime <= 0)
        {
            behaviorComplete = true;
        }
    }

    public GrappleLashState()
    {
        stateID = FSMStateID.GrappleLash;
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Dead Check
        if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= 0)
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
        Range = 0.0f;
        animtime = 1.0f;
        stateMachine = self.GetComponent<BossEnemy>();
        behaviorComplete = false;
        this.player = player.GetComponent<PlayerController>();
        grabDel = GrabPlayer;

        foreach (Animator tentacle in stateMachine.tentacles)
        {
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
                }
            }
        }

        tentacleHead = stateMachine.tentacleColliders[chosenTent][7];
        tentHealth = chosenTent.GetComponent<HealthScript>();
        tentHealth.onCollidePlayer.AddListener(grabDel);

        stateMachine.StartCoroutine(GrabAnim());

        Debug.Log("Selected collider is " + tentacleHead, tentacleHead);
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        chosenTent.SetBool("IsGrapple", false);
        chosenTent.SetBool("isHorizontal", false);
        tentHealth.onCollidePlayer.RemoveListener(grabDel);
    }

    public void GrabPlayer() {
        playerOriginalParent = player.transform.parent;
        player.transform.parent = tentacleHead.transform;
        player.Freeze_Unfreeze();
        success = true;
    }

    public void ReleasePlayer() {
        player.transform.parent = playerOriginalParent;
        player.Freeze_Unfreeze();
    }

    private IEnumerator GrabAnim() {
        chosenTent.SetBool("IsGrapple", true);
        yield return new WaitForSeconds(1);
        if (success)
        {
            stateMachine.StartCoroutine(Throw());
        }
        else { behaviorComplete = true; }
        chosenTent.SetBool("IsGrapple", false);
    }

    private IEnumerator Throw() {
        chosenTent.SetBool("isHorizontal", true);
        yield return new WaitForSeconds(1.14f);
        chosenTent.SetBool("isHorizontal", false);
        ReleasePlayer();
        player.Yeet();
    }
}
