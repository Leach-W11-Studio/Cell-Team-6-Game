using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleLashState : FSMState
{
    private BossEnemy stateMachine;
    private float animtime;
    private float Range;
    private Animator chosenTent;
    private bool initialize = true;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle

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
        chosenTent.SetBool("IsGrapple", true);
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        chosenTent.SetBool("IsGrapple", false);
    }

}
