using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle

    public LashState()
    {
        stateID = FSMStateID.Lash;
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
        /* if(self.GetComponent<BossEnemy>().healthScript.currentHealth <= self.GetComponent<BossEnemy>().Phase2Threshold)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.Phase2LashRange);
        } Why tho? */
        behaviorComplete = false;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

}
