using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackState : FSMState
{
    private float attackSpeed;
    private float projectileVelocity;
    private float elapsed;
    private HealthScript health;

    private BossEnemy stateMachine;
    public override void Act(Transform player, GameObject self)
    {
        elapsed += Time.deltaTime;
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        health = self.GetComponent<HealthScript>();

        elapsed = 0f;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (health.currentHealth <= 0 || health.isDead) {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        if (elapsed < stateMachine.shootTime) {
            stateMachine.SetTransition(FSMTransitions.none);
        }
    }
}
