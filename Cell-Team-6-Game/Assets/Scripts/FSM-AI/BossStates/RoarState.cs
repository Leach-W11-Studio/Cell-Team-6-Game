using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoarState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private BossEnemy stateMachine;
    private float animtime;
    private float position;
    private float Range;
    private bool initialize;
    private float Delay;
    //private Animator chosenTent;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle

    public override void Act(Transform player, GameObject self)
    {
        animtime -= Time.deltaTime;
        if (animtime <= 0)
        {
            behaviorComplete = true;
        }
    }

    public RoarState()
    {
        stateID = FSMStateID.Roar;
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
        Range = 40.0f;
        animtime = 2.0f;
        Delay = 1.0f;
        stateMachine = self.GetComponent<BossEnemy>();
        behaviorComplete = false;

        //stateMachine.CoreAnim.SetTrigger("RoarAnim");
        stateMachine.StartCoroutine(AttackDelay(player, self));
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        /* chosenTent.SetBool("IsVertical", false);
        chosenTent.GetComponent<HealthScript>().invincible = true; */
        /* foreach (Animator tentacle in stateMachine.tentacles)
        {
            if (tentacle != chosenTent)
            {
                //Set both avoidance animations here
            }
        } */

        /*foreach (CircleCollider2D bone in stateMachine.tentacleColliders[chosenTent])
        {
            bone.enabled = false;
        }*/
    }

    private IEnumerator AttackDelay(Transform player, GameObject self)
    {
        yield return new WaitForSeconds(Delay);
        Debug.Log("RAGH!!!");
        if (Vector2.Distance(player.position, self.transform.position) <= Range)
        {
            player.GetComponent<PlayerController>().Yeet(10,2);
            player.GetComponent<PlayerController>().Freeze_Unfreeze();
            stateMachine.StartCoroutine(PlayerUnfreezeDelay(player));
        }
    }

    private IEnumerator PlayerUnfreezeDelay(Transform player)
    {
        yield return new WaitForSeconds(2.0f);
        player.GetComponent<PlayerController>().Freeze_Unfreeze();
    }

}