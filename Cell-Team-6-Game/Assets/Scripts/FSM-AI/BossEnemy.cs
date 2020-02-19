using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : FSM
{
    public HealthScript healthScript;
    protected override void Initalize()
    {
        //currentHealth = initalHealth;
        healthScript = GetComponent<HealthScript>();
        BuildFSM();
    }
    protected virtual void BuildFSM()
    {
        BossIdleState bossIdle = new BossIdleState();
        //[State Name].AddTransitionState([State to transition to], [Transition Name]);

        GrappleLashState grappleLash = new GrappleLashState();

        LashState lashState = new LashState();

        ProjectileAttackState projectileAttack = new ProjectileAttackState();

        TrackRoundState trackRoundState = new TrackRoundState();

        WallSpawnState wallSpawnState = new WallSpawnState();

        DeadState dead = new DeadState();


        AddFSMState(bossIdle);
        AddFSMState(lashState);
        AddFSMState(grappleLash);
        AddFSMState(projectileAttack);
        AddFSMState(trackRoundState);
        AddFSMState(wallSpawnState);
        AddFSMState(dead);
    }

    protected override void FSMUpdate()
    {
        //throw new System.NotImplementedException();
    }

    protected override void FSMFixedUpdate()
    {
        //throw new System.NotImplementedException();
    }
}
