using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle

    public LungeState()
    {
        stateID = FSMStateID.Lunge;
    }

    public override void Act(Transform player, GameObject self)
    {
        //Behavior for Lash Attack here. Note that this is only the Lash attack. Decisions happen elsewhere
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
        behaviorComplete = false;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

}
