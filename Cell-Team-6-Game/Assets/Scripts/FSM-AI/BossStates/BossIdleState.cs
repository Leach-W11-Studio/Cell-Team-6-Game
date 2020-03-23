using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthScript))]
public class BossIdleState : FSMState
{
    private HealthScript health;
    private BossEnemy stateMachine;
    private float elapsed;

    Vector2 lastPlayerPos;

    public BossIdleState()
    {
        stateID = FSMStateID.BossIdle;
    }

    public override void Act(Transform player, GameObject self)
    {
        elapsed += Time.deltaTime;
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        health = self.GetComponent<HealthScript>();
        stateMachine = self.GetComponent<BossEnemy>();

        foreach (Animator tentacle in stateMachine.tentacles)
        {
            tentacle.Play("Idle", 0, Random.Range(0, 1));
        }

        elapsed = 0;
        //Lots of different transitions will need to be implemented here
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (health.isDead || health.currentHealth <= 0)
        {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        else if (stateMachine.RadRangeCheck(player) == Radius.Rad1)
        {
            stateMachine.SetTransition(FSMTransitions.InRad1);
        }
        else if (stateMachine.RadRangeCheck(player) == Radius.Rad2)
        {
            stateMachine.SetTransition(FSMTransitions.InRad2);
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
