using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private BossEnemy stateMachine;
    private float RandomTent;
    private Vector2 playerpos;

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
        stateMachine = self.GetComponent<BossEnemy>();
        RandomTent = Mathf.Round(Random.Range(0, stateMachine.tentacles.Count));
        playerpos.x = player.transform.position.x - self.transform.position.x;
        playerpos.y = player.transform.position.y - self.transform.position.y;
        float newRot = Mathf.Atan2(playerpos.y, playerpos.x) * Mathf.Rad2Deg;
        newRot -= 90.0f;
        stateMachine.tentacles[(int)RandomTent].transform.rotation = Quaternion.Euler(new Vector3(0, 0, newRot));
        stateMachine.tentacles[(int)RandomTent].Play("Lunge");
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

}
