using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class BossIdleStatePhase2 : BossIdleState
{

    private bool animDone = false;
    public BossIdleStatePhase2()
    {
        stateID = FSMStateID.BossIdlePhase2;
    }
    public BossIdleStatePhase2(int numBeforeRoar)
    {
        stateID = FSMStateID.BossIdlePhase2;
        roarThreshold = numBeforeRoar;
    }

    private float GetPercentWalls()
    {
        int activeWalls = 0;
        foreach (var wall in stateMachine.bossWallList)
        {
            if (wall.isActive) { activeWalls++; }
        }

        return activeWalls / stateMachine.bossWallList.Count;
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (animDone)
        {
            if (!stateMachine) { return; }
            //Health Checks
            if (health.isDead || health.currentHealth <= 0)
            {
                stateMachine.SetTransition(FSMTransitions.OutOfHealth);
            }

            //Roar Check
            else if (attacksSinceLastRoar >= roarThreshold)
            {
                attacksSinceLastRoar = 0;
                stateMachine.SetTransition(FSMTransitions.PlayerInRangeTooLong);
            }

            //Range Checks - This only chooses whether to shoot or ready lash. Logic for choosing lash is in LashReadyState.
            else if (GetPercentWalls() <= stateMachine.wallSpawnThreshold && stateMachine.timeSinceWallSpawn >= stateMachine.wallSpawnInterval)
            {
                stateMachine.SetTransition(FSMTransitions.WallSpawnTriggered);
            }
            else if (stateMachine.RadRangeCheck(player) == Radius.Rad2 || stateMachine.RadRangeCheck(player) == Radius.Rad1)
            {
                attacksSinceLastRoar++;
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
        }

        lastPlayerPos = player.position; //Is this referenced by anything?
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        health = self.GetComponent<HealthScript>();
        stateMachine = self.GetComponent<BossEnemy>();

        stateMachine.StartCoroutine(StartAnimation());

        elapsed = 0;

        if (player.GetComponent<PlayerController>().frozen)
        {
            player.GetComponent<PlayerController>().Freeze_Unfreeze();
        }
        //Lots of different transitions will need to be implemented here
    }

    private void StopAnimation()
    {
        foreach (Animator tentacle in stateMachine.tentacles)
        {
            tentacle.SetBool("Idle", false);
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        StopAnimation();
    }

    private IEnumerator StartAnimation()
    {
        animDone = false;

        float timeRange = 1f;
        foreach (Animator tentacle in stateMachine.tentacles)
        {
            if (!tentacle) { continue; }
            tentacle.GetComponent<HealthScript>().invincible = true;
            yield return new WaitForSeconds(Random.Range(0, timeRange));
            if (!tentacle) { continue; }
            tentacle.SetBool("Idle", true);
        }

        animDone = true;
    }
}
