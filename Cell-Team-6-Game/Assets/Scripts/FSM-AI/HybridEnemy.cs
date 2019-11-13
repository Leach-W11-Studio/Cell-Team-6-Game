using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HybridEnemy : BaseEnemy
{
    public List<GameObject> PatrolPoints;
    public float chargeDistance = 4f;

    protected override void BuildFSM()
    {
        PatrolState patrol = new PatrolState(PatrolPoints, navAgent, agroDistance);
        patrol.AddTransitionState(FSMStateID.Shoot, FSMTransitions.SawPlayer);
        patrol.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        ShootState shoot = new ShootState(chargeDistance, spawnerScript);
        shoot.AddTransitionState(FSMStateID.Patrol, FSMTransitions.PlayerOutOfRange);
        shoot.AddTransitionState(FSMStateID.Chase, FSMTransitions.PlayerTooClose);
        shoot.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        ChaseState chase = new ChaseState(navAgent, chargeDistance);
        chase.AddTransitionState(FSMStateID.Shoot, FSMTransitions.PlayerOutOfRange);
        chase.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        DeadState dead = new DeadState();

        AddFSMState(patrol);
        AddFSMState(shoot);
        AddFSMState(chase);
        AddFSMState(dead);
    }
}
