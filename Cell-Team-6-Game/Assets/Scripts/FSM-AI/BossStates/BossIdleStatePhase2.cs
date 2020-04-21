using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BossIdleStatePhase2 : BossIdleState
{
    public BossIdleStatePhase2()
    {
        stateID = FSMStateID.BossIdlePhase2;
    }

    private float GetPercentWalls() {
        int activeWalls = 0;
        foreach (var wall in stateMachine.bossWallList) {
            if (wall.isActive) { activeWalls++; }
        }

        return activeWalls / stateMachine.bossWallList.Count;
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Health Checks
        if (health.isDead || health.currentHealth <= 0)
        {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        //Range Checks - This only chooses whether to shoot or ready lash. Logic for choosing lash is in LashReadyState.
        else if (GetPercentWalls() <= stateMachine.wallSpawnThreshold && stateMachine.timeSinceWallSpawn >= stateMachine.wallSpawnInterval)
        {
            stateMachine.SetTransition(FSMTransitions.WallSpawnTriggered);
        }
        else if (stateMachine.RadRangeCheck(player) == Radius.Rad2)
        {
            stateMachine.SetTransition(FSMTransitions.InMeleeRange);
        }
        else if (stateMachine.RadRangeCheck(player) == Radius.Rad3)
        {
            stateMachine.SetTransition(FSMTransitions.GreaterThanRad2);
        }

        /* else { WHYYYYY
            if (elapsed < stateMachine.shootInterval) {
                stateMachine.SetTransition(FSMTransitions.Shoot);
            }
        } */

        lastPlayerPos = player.position; //Is this referenced by anything?
    }
}
