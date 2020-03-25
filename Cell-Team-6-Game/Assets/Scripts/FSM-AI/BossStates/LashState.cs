using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LashState : FSMState
{
    private float outOfRange;
    private float attackSpeed;
    private BossEnemy stateMachine;
    private float RandomTent;

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
        if (Vector3.Distance(self.transform.position, player.position) > self.GetComponent<BossEnemy>().lashDistance)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.PlayerOutOfRange);  
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        stateMachine = self.GetComponent<BossEnemy>();
        RandomTent = Mathf.Round(Random.Range(0, stateMachine.tentacles.Count));
        stateMachine.tentacles[(int)RandomTent].Play("Lash");

        /*if (self.GetComponent<BossEnemy>().healthScript.currentHealth <= self.GetComponent<BossEnemy>().Phase2Threshold)
        {
            self.GetComponent<BossEnemy>().SetTransition(FSMTransitions.Phase2LashRange);
        }*/
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        throw new System.NotImplementedException();
    }

}
