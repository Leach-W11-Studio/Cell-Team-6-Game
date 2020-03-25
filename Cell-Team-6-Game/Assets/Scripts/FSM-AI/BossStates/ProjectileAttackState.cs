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
    private float shootCone = 0.5f; //Value between 0 and 1;
    private float lastShot;

    public override void Act(Transform player, GameObject self)
    {
        elapsed += Time.deltaTime;

        if (elapsed - lastShot > stateMachine.shootInterval) {
            Shoot(self.transform);
        }
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= 0)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        if (elapsed > stateMachine.shootTime) {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.none);
        }
        if (Vector3.Distance(self.transform.position, player.position) < self.GetComponent<BossEnemy>().projectileDistance)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.PlayerTooClose);
        }

    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        health = self.GetComponent<HealthScript>();

        Debug.Log("Transitioning to Projectile State", self);
        stateMachine.StartCoroutine(StartAnimation());
        elapsed = 0f;

        if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= self.GetComponent<BossEnemy>().Phase2Threshold)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.Phase2ProjectileRange);
        }
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        StopAnimation();
        if (health.currentHealth <= 0 || health.isDead) {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        if (elapsed < stateMachine.shootTime) {
            stateMachine.SetTransition(FSMTransitions.none);
        }
    }

    private void Shoot(Transform self) {
        Vector2 shootVector = RandomShootVector(self);
        Debug.DrawRay(self.position, shootVector * 50, Color.yellow);
    }

    private Vector2 RandomShootVector(Transform self) {
        Vector2 down = -self.up;
        Vector2 perp = Vector2.Perpendicular(down);
        int direction = Random.Range(-1f, 1f) >= 0 ? 1 : -1; // Basically a coin flip. ensures a random choice of 1 or -1;
        Vector2 randomVector = Vector2.Lerp(down, direction * perp, Random.Range(0, shootCone)).normalized;

        return randomVector;
    }

    private void StopAnimation() {
        foreach(Animator tentacle in stateMachine.tentacles) {
            tentacle.SetBool("Shooting", false);
        }
    }

    private IEnumerator StartAnimation() {
        float timeRange = 1f;
        foreach(Animator tentacle in stateMachine.tentacles) {
            yield return new WaitForSeconds(Random.Range(0, timeRange));
            tentacle.SetBool("Shooting", true);
        }
    }
}
