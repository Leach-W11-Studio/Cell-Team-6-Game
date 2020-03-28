using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private BossEnemy stateMachine;
    private float RandomTent;
    private float animtime;
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
            parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        animtime = 1.5f;
        stateMachine = self.GetComponent<BossEnemy>();
        RandomTent = Mathf.Round(Random.Range(0, stateMachine.tentacles.Count));
        stateMachine.tentacles[(int)RandomTent].Play("HorizontalSlash");

        behaviorComplete = false;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

}
