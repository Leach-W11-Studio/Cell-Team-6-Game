using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : BaseEnemy
{
    protected override void BuildFSM()
    {
        PassiveState passive = new PassiveState(activateRaidus);
        passive.AddTransitionState(FSMStateID.Chase, FSMTransitions.Awoken);
        passive.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        IdleState idle = new IdleState();
        idle.AddTransitionState(FSMStateID.Chase, FSMTransitions.SawPlayer);
        idle.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        ChaseState chase = new ChaseState(navAgent, agroDistance);
        chase.AddTransitionState(FSMStateID.Idle, FSMTransitions.PlayerOutOfRange);
        chase.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        DeadState dead = new DeadState();

        AddFSMState(passive);
        AddFSMState(idle);
        AddFSMState(chase);
        AddFSMState(dead);
    }
}
