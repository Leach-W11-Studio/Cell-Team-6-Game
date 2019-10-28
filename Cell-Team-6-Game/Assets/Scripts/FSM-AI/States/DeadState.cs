using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : FSMState
{
    public DeadState()
    {
        stateID = FSMStateID.Dead;
    }

    public override void Act(Transform player, GameObject self)
    {
        //Destroy(self, 1f);
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Can Never Leave DeadState - No Transitions
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {

    }

    public override void OnStateExit(Transform player, GameObject self)
    {

    }
}
