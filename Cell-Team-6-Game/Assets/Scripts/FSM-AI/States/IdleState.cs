using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{
    public IdleState()
    {
        stateID = FSMStateID.Idle;
    }

    public override void Act(Transform player, GameObject self)
    {
        //Does nothing in this state. Act is therefore empty as of now.
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (self.GetComponent<BaseEnemy>().healthScript.currentHealth <= 0)
        {
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        else if (Vector2.Distance(self.transform.position, player.position) <= self.GetComponent<BaseEnemy>().agroDistance)
        {
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.SawPlayer);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {

    }

    public override void OnStateExit(Transform player, GameObject self)
    {

    }
}
