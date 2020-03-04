using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeState : FSMState
{
    private float outOfRange;
    private float attackSpeed;

    public override void Act(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= 0)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }

        if (Vector3.Distance(self.transform.position, player.position) > self.GetComponent<BossEnemy>().projectileDistance)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.PlayerOutOfRange);
        }

        if (Vector3.Distance(self.transform.position, player.position) < self.GetComponent<BossEnemy>().lashDistance)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.PlayerTooClose);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

}
