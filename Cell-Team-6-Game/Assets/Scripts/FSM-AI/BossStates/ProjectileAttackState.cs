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
    private float toocloseRange;
    private float shootCone; //Value between 0 and 1;
    private float lastShot;
    private bool behaviorComplete; //Set to True when the behavior is complete. This triggers transition back to Idle
    private string bulletName;

    private bool animDone;

    public ProjectileAttackState(string bulletName)
    {
        stateID = FSMStateID.Projectile;
        this.bulletName = bulletName;
    }

    public override void Act(Transform player, GameObject self)
    {
        if (elapsed - lastShot > stateMachine.shootInterval) {
            Shoot(self.transform);
        }
        
        elapsed += Time.deltaTime;
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (animDone)
        {
            //Death Check
            if (health.currentHealth <= 0)
            {
                self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.OutOfHealth);
            }

            //Completion Check
            else if (elapsed > stateMachine.shootTime)
            {
                parentFSM.SetTransition(FSMTransitions.BehaviorComplete);
            }
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        shootCone = stateMachine.shootCone;
        health = self.GetComponent<HealthScript>();
        Debug.Log("Boss in projectile state", health);
        stateMachine.StartCoroutine(StartAnimation());
        elapsed = 0f;
        lastShot = 0f;

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
        StopAnimation();
        /* if (health.currentHealth <= 0 || health.isDead) {
            stateMachine.SetTransition(FSMTransitions.OutOfHealth);
        }

        if (elapsed < stateMachine.shootTime) {
            stateMachine.SetTransition(FSMTransitions.none);
        } Same as above. Also never use FSMTransitions.none. That's a placeholder and will by design cause an error everytime it is used */
    }

    private void Shoot(Transform self) {
        lastShot = elapsed;
        Vector2 shootVector = RandomShootVector(self);
        Debug.DrawRay(self.position, shootVector * 50, Color.yellow);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootVector);
        ObjectQueue.Instance.SpawnFromPool(bulletName, stateMachine.muzzle.position, rotation);
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
        animDone = false;
        foreach(Animator tentacle in stateMachine.tentacles) {
            yield return new WaitForSeconds(Random.Range(0, timeRange));
            if (tentacle) { 
                tentacle.SetBool("Shooting", true);
            }
        }

        animDone = true;
    }
}
