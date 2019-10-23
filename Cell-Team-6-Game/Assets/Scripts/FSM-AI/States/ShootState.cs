using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : FSMState
{
    float retreatDistance;

    public ShootState()
    {
        stateID = FSMStateID.Shoot;
        retreatDistance = 0;
    }

    /// <param name="distance">Distance at which this enemy begins to retreat, Set to 0 or do not include for no retreating.</param>
    public ShootState(float distance)
    {
        stateID = FSMStateID.Shoot;
        retreatDistance = distance;
    }

    public override void Act(Transform player, GameObject self)
    {
        Vector2 heading = player.position - self.transform.position;
        heading.Normalize();
        float zRot = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        self.transform.rotation = Quaternion.Lerp(self.transform.rotation, Quaternion.Euler(0f, 0f, zRot), Time.fixedDeltaTime * self.GetComponent<BaseEnemy>().rotationSpeed);
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (self.GetComponent<BaseEnemy>().spawnerScript.shoot != true) { self.GetComponent<BaseEnemy>().spawnerScript.shoot = true; }

        if (self.GetComponent<BaseEnemy>().healthScript.currentHealth <= 0)
        {
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        else if (Vector2.Distance(self.transform.position, player.position) > self.GetComponent<BaseEnemy>().agroDistance)
        {
            self.GetComponent<BaseEnemy>().spawnerScript.shoot = false;
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.PlayerOutOfRange);
        }
        else if(retreatDistance != 0 && Vector2.Distance(self.transform.position, player.position) <= retreatDistance)
        {
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.PlayerTooClose);
        }
    }
}
