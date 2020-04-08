using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExProjectileAttack : FSMState
{
    private float elapsed;
    private HealthScript health;

    private BossEnemy stateMachine;
    private float lastShot;
    private bool animDone;
    int tentacleSelector;

    private Transform muzzle;

    public ExProjectileAttack()
    {
        stateID = FSMStateID.EXProjectile;
    }

    public override void Act(Transform player, GameObject self)
    {
        if (elapsed - lastShot > stateMachine.shootInterval) {
            Shoot(muzzle.transform);
        }
        
        elapsed += Time.deltaTime;
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        health = self.GetComponent<HealthScript>();
        Debug.Log("Boss in projectile state", health);
        StartAnimation();
        elapsed = 0f;
        lastShot = 0f;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        for (int i = 0; i < stateMachine.tentacles.Count; i++)
        {
            if (i == tentacleSelector) { stateMachine.tentacles[i].SetBool("IsHackShoot", false); }
            else { stateMachine.tentacles[i].SetBool("Idle", false); }
        }
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

    private void StartAnimation()
    {
        //tentacleSelector = Random.Range(0, stateMachine.tentacles.Count);
        tentacleSelector = 2;
        muzzle = stateMachine.tentacles[tentacleSelector].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);

        for (int i = 0; i < stateMachine.tentacles.Count; i++)
        {
            if (i == tentacleSelector) { stateMachine.tentacles[i].SetBool("IsHackShoot", true); }
            else { stateMachine.tentacles[i].SetBool("Idle", true); }
        }

        animDone = true;
    }

    private void Shoot(Transform self) {
        lastShot = elapsed;
        /* Vector2 shootVector = RandomShootVector(self);
        Debug.DrawRay(self.position, shootVector * 50, Color.yellow);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootVector); */
        Vector3 rot = self.transform.rotation.eulerAngles;
        rot.z-=45f;
        Quaternion rotation = Quaternion.Euler(rot);

        ObjectQueue.Instance.SpawnFromPool("BossBulletPhase1", self.transform.position, rotation);
    }
}
