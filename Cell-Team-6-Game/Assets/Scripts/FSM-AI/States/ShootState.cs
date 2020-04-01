using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : FSMState
{
    float retreatDistance;
    private BulletSpawnerScript gun;
    //private HealthScript selfHealthScript; - Depricated
    private BaseEnemy parentEnemyReference;
    private float rotationSpeed;

    public ShootState(BulletSpawnerScript bulletSpawner)
    {
        stateID = FSMStateID.Shoot;
        retreatDistance = 0;
        gun = bulletSpawner;
    }

    /// <param name="distance">Distance at which this enemy begins to retreat, Set to 0 or do not include for no retreating.</param>
    public ShootState(float distance, BulletSpawnerScript bulletSpawner)
    {
        stateID = FSMStateID.Shoot;
        retreatDistance = distance;
        gun = bulletSpawner;
    }

    public override void Act(Transform player, GameObject self)
    {
        Vector2 heading = player.position - self.transform.position;
        heading.Normalize();
        float zRot = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        self.transform.rotation = Quaternion.Lerp(self.transform.rotation, Quaternion.Euler(0f, 0f, zRot), Time.fixedDeltaTime * rotationSpeed);
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (parentEnemyReference.healthScript.currentHealth <= 0)
        {
            parentFSM.SetTransition(FSMTransitions.OutOfHealth);
        }
        else if (Vector2.Distance(self.transform.position, player.position) > parentEnemyReference.agroDistance)
        {
            parentFSM.SetTransition(FSMTransitions.PlayerOutOfRange);
        }
        else if (retreatDistance != 0 && Vector2.Distance(self.transform.position, player.position) <= retreatDistance)
        {
            parentFSM.SetTransition(FSMTransitions.PlayerTooClose);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        gun.shoot = true;
        parentEnemyReference = self.GetComponent<BaseEnemy>();
        rotationSpeed = parentEnemyReference.rotationSpeed;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        gun.shoot = false;
    }
}
