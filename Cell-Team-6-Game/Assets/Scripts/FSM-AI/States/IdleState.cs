using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{
    public IdleState()
    {
        stateID = FSMStateID.Idle;
    }

    private BaseEnemy parentEnemyReference;

    public override void Act(Transform player, GameObject self)
    {
        //Does nothing in this state. Act is therefore empty as of now.
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (parentEnemyReference.healthScript.currentHealth <= 0)
        {
            parentFSM.SetTransition(FSMTransitions.OutOfHealth);
        }
        else if (Vector2.Distance(self.transform.position, player.position) <= parentEnemyReference.agroDistance)
        {
            parentFSM.SetTransition(FSMTransitions.SawPlayer);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        parentEnemyReference = self.GetComponent<BaseEnemy>();
    }

    public override void OnStateExit(Transform player, GameObject self)
    {

    }
}
