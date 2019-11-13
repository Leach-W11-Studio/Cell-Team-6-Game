using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
    private PolyNavAgent agent;
    private float agroRange;
    private HealthScript selfHealthScript;
    private float rotationSpeed;

    public ChaseState(PolyNavAgent navAgent, float terminalDistance)
    {
        stateID = FSMStateID.Chase;
        agent = navAgent;

        agroRange = terminalDistance;
    }

    public override void Act(Transform player, GameObject self)
    {
        agent.SetDestination(player.position);

        Vector2 heading = player.position - self.transform.position;
        heading.Normalize();
        float zRot = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        self.transform.rotation = Quaternion.Lerp(self.transform.rotation, Quaternion.Euler(0f, 0f, zRot), Time.fixedDeltaTime * rotationSpeed);
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (selfHealthScript.currentHealth <= 0)
        {
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        else if (Vector2.Distance(self.transform.position, player.position) > agroRange)
        {
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.PlayerOutOfRange);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        selfHealthScript = self.GetComponent<BaseEnemy>().healthScript;
        rotationSpeed = self.GetComponent<BaseEnemy>().rotationSpeed;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {
        //Debug.Log("ExitTest");
    }
}
