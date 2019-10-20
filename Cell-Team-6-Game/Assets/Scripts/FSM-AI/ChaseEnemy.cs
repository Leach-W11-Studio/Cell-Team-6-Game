using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : BaseEnemy
{
    protected override void BuildFSM()
    {
        IdleState idle = new IdleState();
        idle.AddTransitionState(FSMStateID.Chase, FSMTransitions.SawPlayer);
        idle.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        ChaseState chase = new ChaseState(navAgent);
        chase.AddTransitionState(FSMStateID.Idle, FSMTransitions.PlayerOutOfRange);
        chase.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        DeadState dead = new DeadState();

        AddFSMState(idle);
        AddFSMState(chase);
        AddFSMState(dead);
    }
}
