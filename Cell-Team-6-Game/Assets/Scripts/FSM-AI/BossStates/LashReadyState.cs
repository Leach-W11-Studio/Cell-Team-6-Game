using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashReadyState : FSMState
{
    /// <summary>
    /// Constructor for the Lash State
    /// </summary>
    /// <param name="shootChance">Chance that the boss will shoot instead of lashing. Will be ignored if player is within rad1. Provide Int between 0 and 100</param>
    public LashReadyState(int shootChance)
    {
        stateID = FSMStateID.LashReady;
        chanceToShoot = shootChance;
    }

    int shootCheck;
    int chanceToShoot;

    private BossEnemy stateMachine;

    public override void Act(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        shootCheck = Random.Range(0, 100);
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        Radius playerRad = stateMachine.RadRangeCheck(player);

        //Obligitory Dead Check
        if (stateMachine.coreHealthScript.isDead || stateMachine.coreHealthScript.currentHealth <= 0)
        {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        //Behavior Checks
        else if (playerRad == Radius.Rad1)
        {
            stateMachine.SetTransition(FSMTransitions.InRad1);
        }
        else if (shootCheck <= chanceToShoot)
        {
            stateMachine.SetTransition(FSMTransitions.OORad1AndChance);
        }
        else
        {
            stateMachine.SetTransition(FSMTransitions.InRad2);
        }
    }
}
