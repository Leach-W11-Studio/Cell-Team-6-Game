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
    private float toocloseRange;

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
        stateMachine = self.GetComponent<BossEnemy>();
        health = self.GetComponent<HealthScript>();

        elapsed = 0f;
        if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= 0)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        if (Vector3.Distance(self.transform.position, player.position) < self.GetComponent<BossEnemy>().projectileDistance)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.PlayerTooClose);
        }

    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= self.GetComponent<BossEnemy>().Phase2Threshold)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.Phase2ProjectileRange);
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        if (health.currentHealth <= 0 || health.isDead) {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        if (elapsed < stateMachine.shootTime) {
            stateMachine.SetTransition(FSMTransitions.none);
        }
    }
}
