using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRoundState : FSMState
{
    private float attackSpeed;
    private float projectileVelocity;
    private float meleeDistance;
    private bool behaviorComplete;
    private BossEnemy stateMachine;

    public TrackRoundState()
    {
        stateID = FSMStateID.Tracking;
    }

    public override void Act(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        behaviorComplete = false;
        stateMachine = self.GetComponent<BossEnemy>();
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Death Check
        if (stateMachine.healthScript.currentHealth <= 0)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }

        //Completion Check
        else if (behaviorComplete)
        {
            parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
        }
    }
}
