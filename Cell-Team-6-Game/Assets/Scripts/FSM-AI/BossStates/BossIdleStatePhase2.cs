using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleStatePhase2 : BossIdleState
{
    public BossIdleStatePhase2()
    {
        stateID = FSMStateID.BossIdlePhase2;
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Health Checks
        if (health.isDead || health.currentHealth <= 0)
        {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        //Range Checks - This only chooses whether to shoot or ready lash. Logic for choosing lash is in LashReadyState.
        else if (stateMachine.RadRangeCheck(player) == Radius.Rad2)
        {
            stateMachine.SetTransition(FSMTransitions.InMeleeRange);
        }
        else if (stateMachine.RadRangeCheck(player) == Radius.Rad3)
        {
            stateMachine.SetTransition(FSMTransitions.GreaterThanRad2);
        }
        else if (stateMachine.doWallSpawnTrigger)
        {
            stateMachine.SetTransition(FSMTransitions.WallSpawnTriggered);
        }

        /* else { WHYYYYY
            if (elapsed < stateMachine.shootInterval) {
                stateMachine.SetTransition(FSMTransitions.Shoot);
            }
        } */

        lastPlayerPos = player.position; //Is this referenced by anything?
    }
}
