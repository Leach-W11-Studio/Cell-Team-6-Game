using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashReadyState : FSMState
{
    public LashReadyState()
    {
        stateID = FSMStateID.LashReady;
    }

    private BossEnemy stateMachine;

    public override void Act(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }
}
