using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackState : FSMState
{
    //These three should be set in the constructor - Ben
    private float attackSpeed;
    private float projectileVelocity;
    private float elapsed;
    private HealthScript health;

    private BossEnemy stateMachine;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle

    public ProjectileAttackState()
    {
        stateID = FSMStateID.Projectile;
    }

    public override void Act(Transform player, GameObject self)
    {
        elapsed += Time.deltaTime;
    }

    public override void Reason(Transform player, GameObject self)
    {
        elapsed = 0f;
        //Death Check
        if (stateMachine.healthScript.currentHealth <= 0)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }

        //Completion Check
        else if (behaviorComplete)
        {
            parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        /* if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= self.GetComponent<BossEnemy>().Phase2Threshold)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.Phase2ProjectileRange);
        } Please don't do transitions on state enter. This is untested behavior. */
        stateMachine = self.GetComponent<BossEnemy>();
        health = self.GetComponent<HealthScript>();
        behaviorComplete = false;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        /* if (health.currentHealth <= 0 || health.isDead) {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        if (elapsed < stateMachine.shootTime) {
            stateMachine.SetTransition(FSMTransitions.none);
        } Same as above. Also never use FSMTransitions.none. That's a placeholder and will by design cause an error everytime it is used */
    }
}
