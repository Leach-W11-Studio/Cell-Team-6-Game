using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private float whipDistance;
    private float lashDistance;

    public override void Act(Transform player, GameObject self)
    {
        //If within a certain distance use the lash attack
            //Else use lash attack
        throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Factors distance into random choice of wether to switch to projectile state or not
            //IE. If player is further away it is more likely that it will switch to projectile state instead of using whip attack
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

}
