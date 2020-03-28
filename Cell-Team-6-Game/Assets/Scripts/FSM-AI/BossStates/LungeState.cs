using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private BossEnemy stateMachine;
    private float RandomTent;
    private Vector2 playerpos;
    private float animtime;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle
    
    public override void Act(Transform player, GameObject self)
    {
        animtime -= Time.deltaTime;
        if (animtime <= 0)
        {
            behaviorComplete = true;
        }
    }

    public LungeState()
    {
        stateID = FSMStateID.Lunge;
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

    public override void OnStateEnter(Transform player, GameObject self)
    {
        animtime = 1.0f;
        stateMachine = self.GetComponent<BossEnemy>();
        RandomTent = Mathf.Round(Random.Range(0, stateMachine.tentacles.Count));
        /*playerpos.x = player.transform.position.x - self.transform.position.x;
        playerpos.y = player.transform.position.y - self.transform.position.y;
        float newRot = Mathf.Atan2(playerpos.y, playerpos.x) * Mathf.Rad2Deg;
        newRot -= 90.0f;
        stateMachine.tentacles[(int)RandomTent].transform.rotation = Quaternion.Euler(new Vector3(0, 0, newRot));*/
        stateMachine.tentacles[(int)RandomTent].Play("ForwardWhip");
        behaviorComplete = false;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

}
