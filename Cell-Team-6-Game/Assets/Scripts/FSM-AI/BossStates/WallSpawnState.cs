using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawnState : FSMState
{
    private bool behaviorComplete;
    private int wallResetThreshold;
    private List<BossWalls> wallList;
    private BossEnemy stateMachine;

    public WallSpawnState(List<BossWalls> listOfWalls, int wallThreshold)
    {
        stateID = FSMStateID.WallSpawn;
        wallList = listOfWalls;
        wallResetThreshold = wallThreshold;
    }

    public override void Act(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        behaviorComplete = false;

        int activeWallCounter = 0;
        foreach (var wall in wallList)
        {
            if (wall.isActive) { activeWallCounter++; }
        }
        if (activeWallCounter <= wallResetThreshold)
        {
            //Put animation trigger here once we have an animation
            foreach (var wall in wallList)
            {
                if (!wall.isActive) { wall.Enable(); }
            }
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Death Check
        if (stateMachine.coreHealthScript.currentHealth <= 0)
        {
            parentFSM.SetTransition(FSMTransitions.OutOfHealth);
        }

        //Completion Check
        else if (behaviorComplete)
        {
            parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
        }
    }
}
