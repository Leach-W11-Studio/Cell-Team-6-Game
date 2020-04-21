using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class WallSpawnState : FSMState
{
    private bool behaviorComplete;
    private float wallResetThreshold;
    private List<BossWalls> wallList;
    private BossEnemy stateMachine;

    public WallSpawnState(List<BossWalls> listOfWalls)
    {
        stateID = FSMStateID.WallSpawn;
        wallList = listOfWalls;
    }

    public override void Act(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        stateMachine.timeSinceWallSpawn = 0;
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

        behaviorComplete = true;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Death Check
        if (stateMachine.healthScript.currentHealth <= 0)
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
