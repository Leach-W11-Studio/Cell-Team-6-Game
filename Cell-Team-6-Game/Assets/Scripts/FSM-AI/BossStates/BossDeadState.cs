using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : FSMState
{
    private BossEnemy stateMachine;
    public BossDeadState()
    {
        stateID = FSMStateID.BossDead;
    }
    public override void Act(Transform player, GameObject self)
    {
    }

    public override void Reason(Transform player, GameObject self)
    {
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        foreach(Animator tentacle in stateMachine.tentacles) {
            tentacle.SetTrigger("Die");
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
    }

}
