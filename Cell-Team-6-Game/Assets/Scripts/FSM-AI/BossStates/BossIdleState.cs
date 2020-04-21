using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthScript))]
public class BossIdleState : FSMState
{
    protected HealthScript health;
    protected BossEnemy stateMachine;
    protected float elapsed;

    protected int attacksSinceLastRoar;
    protected int roarThreshold;

    protected Vector2 lastPlayerPos;

    private bool animDone;

    public BossIdleState()
    {
        stateID = FSMStateID.BossIdle;
        roarThreshold = 5;
    }
    public BossIdleState(int numBeforeRoar)
    {
        stateID = FSMStateID.BossIdle;
        roarThreshold = numBeforeRoar;
    }
    

    public override void Act(Transform player, GameObject self)
    {
        elapsed += Time.deltaTime;
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        health = self.GetComponent<HealthScript>();
        stateMachine = self.GetComponent<BossEnemy>();

        stateMachine.StartCoroutine(StartAnimation());

        elapsed = 0;
        //Lots of different transitions will need to be implemented here
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        StopAnimation();
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (animDone)
        {
            //Health Checks
            if (health.isDead || health.currentHealth <= 0)
            {
                stateMachine.SetTransition(FSMTransitions.OutOfHealth);
            }
            else if (health.currentHealth <= stateMachine.Phase2Threshold)
            {
                stateMachine.SetTransition(FSMTransitions.HealthLessThanThreshold);
            }

            //Roar Check
            else if (attacksSinceLastRoar >= roarThreshold)
            {
                attacksSinceLastRoar = 0;
                stateMachine.SetTransition(FSMTransitions.PlayerInRangeTooLong);
            }

            //Range Checks - This only chooses whether to shoot or ready lash. Logic for choosing lash is in LashReadyState.
            else if (stateMachine.RadRangeCheck(player) == Radius.Rad2 || stateMachine.RadRangeCheck(player) == Radius.Rad1)
            {
                attacksSinceLastRoar++;
                stateMachine.SetTransition(FSMTransitions.InMeleeRange);
            }
            else if (stateMachine.RadRangeCheck(player) == Radius.Rad3)
            {
                stateMachine.SetTransition(FSMTransitions.GreaterThanRad2);
            }
        }

        /* else { WHYYYYY
            if (elapsed < stateMachine.shootInterval) {
                stateMachine.SetTransition(FSMTransitions.Shoot);
            }
        } */

        lastPlayerPos = player.position; //Is this referenced by anything?
    }

    private void StopAnimation() {
        foreach (Animator tentacle in stateMachine.tentacles)
        {
            tentacle.SetBool("Idle", false);
        }
    }

    private IEnumerator StartAnimation() {
        float timeRange = 1f;
        animDone = false;
        foreach(Animator tentacle in stateMachine.tentacles) {
            tentacle.GetComponent<HealthScript>().invincible = true;
            yield return new WaitForSeconds(Random.Range(0, timeRange));
            tentacle.SetBool("Idle", true);
        }

        animDone = true;
    }
}
