﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRoundState : FSMState
{
    private float attackSpeed;
    private float projectileVelocity;
    private float meleeDistance;

    public TrackRoundState()
    {
        stateID = FSMStateID.Tracking;
    }

    public override void Act(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }
}
