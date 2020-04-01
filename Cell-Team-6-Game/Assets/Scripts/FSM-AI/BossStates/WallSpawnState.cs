using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawnState : FSMState
{
    private BossEnemy stateMachine;
    private float animtime;
    private bool behaviorComplete;
    
    public WallSpawnState()
    {
        stateID = FSMStateID.WallSpawn;
    }

    public override void Act(Transform player, GameObject self)
    {
        animtime -= Time.deltaTime;
        if (animtime <= 0)
        {
            stateMachine = self.GetComponent<BossEnemy>();
            foreach(GameObject wall in stateMachine.spawnWalls)
            {
                wall.SetActive(true);
            }
            behaviorComplete = true;
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        animtime = 1.0f;
        behaviorComplete = false;
        stateMachine = self.GetComponent<BossEnemy>();
        foreach (Animator tentacle in stateMachine.tentacles)
        {
            tentacle.SetBool("IsWall", true);
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        StopAnimation();
    }

    public override void Reason(Transform player, GameObject self)
    {
        //Dead Check
        if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= 0)
        {
            parentFSM.SetTransition(FSMTransitions.OutOfHealth);
        }

        //Completion Check
        else if (behaviorComplete)
        {
            parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
        }
    }

    public void StopAnimation()
    {
        foreach (Animator tentacle in stateMachine.tentacles)
        {
            tentacle.SetBool("IsWall", false);
        }
    }
}
