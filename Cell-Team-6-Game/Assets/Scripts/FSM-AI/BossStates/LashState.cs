using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private BossEnemy stateMachine;
    private float animtime;
    private float position;
    private float Range;
    private Animator chosenTent;
    private bool initialize;
    private float Delay;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle

    public override void Act(Transform player, GameObject self)
    {
        animtime -= Time.deltaTime;
        if(animtime <= 0)
        {
            behaviorComplete = true;
        }
    }
    public LashState()
    {
        stateID = FSMStateID.Lash;
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
            Delay -= Time.deltaTime;
            if (Delay <= 0)
                parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        Range = 0.0f;
        animtime = 1.5f;
        Delay = 1.0f;
        stateMachine = self.GetComponent<BossEnemy>();
        behaviorComplete = false;
        initialize = true;

        foreach (Animator tentacle in stateMachine.tentacles)
        {
            if (initialize == true)
            {
                Range = Vector2.Distance(tentacle.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position, player.position);
                initialize = false;
                chosenTent = tentacle;
            }
            else
            {
                if (Vector2.Distance(tentacle.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position, player.position) < Range)
                {
                    Range = Vector2.Distance(tentacle.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform.position, player.position);
                    chosenTent = tentacle;
                }
            }
        }

        foreach (Animator tentacle in stateMachine.tentacles)
        {
            if (tentacle != chosenTent)
            {
                position = tentacle.transform.position.x - chosenTent.transform.position.x;
                if (position > 0.0f)
                {
                    //Set right side animation
                }
                else
                {
                    //set left side animation
                }
            }
        }

        chosenTent.SetBool("IsHorizontal", true);
        chosenTent.GetComponent<HealthScript>().invincible = false;
        foreach(CircleCollider2D bone in stateMachine.tentacleColliders[chosenTent])
        {
            bone.enabled = true;
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        chosenTent.SetBool("IsHorizontal", false);
        chosenTent.GetComponent<HealthScript>().invincible = true;
        foreach (Animator tentacle in stateMachine.tentacles)
        {
            if (tentacle != chosenTent)
            {
                //Set both left and right side animations false
            }
        }

        foreach (CircleCollider2D bone in stateMachine.tentacleColliders[chosenTent])
        {
            bone.enabled = false;
        }
    }

}
