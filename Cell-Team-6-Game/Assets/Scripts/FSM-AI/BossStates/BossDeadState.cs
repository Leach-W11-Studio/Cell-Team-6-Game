using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : FSMState
{
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
        // Start death animation
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
    }

}
