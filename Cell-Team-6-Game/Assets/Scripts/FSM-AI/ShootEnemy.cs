using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class ShootEnemy : BaseEnemy
{
    public List<GameObject> PatrolPoints;
    public float retreatDistance = 4f;

    protected override void BuildFSM()
    {
        PatrolState patrol = new PatrolState(PatrolPoints, navAgent, agroDistance);
        patrol.AddTransitionState(FSMStateID.Shoot, FSMTransitions.SawPlayer);
        patrol.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        ShootState shoot = new ShootState(retreatDistance, spawnerScript);
        shoot.AddTransitionState(FSMStateID.Patrol, FSMTransitions.PlayerOutOfRange);
        shoot.AddTransitionState(FSMStateID.Retreat, FSMTransitions.PlayerTooClose);
        shoot.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        RetreatState retreat = new RetreatState(navAgent, retreatDistance);
        retreat.AddTransitionState(FSMStateID.Shoot, FSMTransitions.CloserDistanceReached);
        retreat.AddTransitionState(FSMStateID.Dead, FSMTransitions.OutOfHealth);

        DeadState dead = new DeadState();

        AddFSMState(patrol);
        AddFSMState(shoot);
        AddFSMState(retreat);
        AddFSMState(dead);
    }
}
