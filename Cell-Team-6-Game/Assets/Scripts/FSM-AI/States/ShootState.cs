using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : FSMState
{
    public ShootState()
    {
        stateID = FSMStateID.Shoot;
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
        if(self.GetComponent<BaseEnemy>().currentHealth <= 0)
        {
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        else if(Vector2.Distance(self.transform.position, player.position) > self.GetComponent<BaseEnemy>().agroDistance)
        {
            self.GetComponent<BaseEnemy>().spawnerScript.shoot = false;
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.PlayerOutOfRange);
        }
    }
}
