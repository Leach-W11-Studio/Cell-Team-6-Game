using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPhase2State : FSMState
{
    private bool behaviorComplete;
    private BossEnemy stateMachine;

    public SetupPhase2State()
    {
        stateID = FSMStateID.Phase2Setup;
    }

    public override void Act(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        stateMachine.RebuildFSMForPhase2();
        behaviorComplete = true; //Currently set to true as soon as enters phase, feel free to change if you want something else to happen.
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
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
}
