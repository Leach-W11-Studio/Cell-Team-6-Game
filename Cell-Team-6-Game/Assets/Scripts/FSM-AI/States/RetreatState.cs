using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : FSMState
{
    private float retreatEndDistance;
    private PolyNavAgent agent;
    private float normalSlowingDistance;
    private HealthScript curHealthScript;
    private float rotSpeed;

    /// <param name="p_retreatEndDistance">Should always be less than agro range</param>
    public RetreatState(PolyNavAgent navAgent, float p_retreatEndDistance)
    {
        stateID = FSMStateID.Retreat;
        agent = navAgent;
        retreatEndDistance = p_retreatEndDistance;
        normalSlowingDistance = agent.slowingDistance;
    }

    public override void Act(Transform player, GameObject self)
    {
        if (agent.slowingDistance == normalSlowingDistance) { agent.slowingDistance = 0; }

        if (agent.remainingDistance <= .5f)
        {
            var targetPosition = self.transform.TransformPoint(-1, 0, 0);
            agent.SetDestination(targetPosition);
        }

        Vector2 heading = player.position - self.transform.position;
        heading.Normalize();
        float zRot = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        self.transform.rotation = Quaternion.Lerp(self.transform.rotation, Quaternion.Euler(0f, 0f, zRot), Time.fixedDeltaTime * rotSpeed);
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (curHealthScript.currentHealth <= 0)
        {
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        else if (Vector2.Distance(self.transform.position, player.position) >= retreatEndDistance)
        {
            agent.slowingDistance = normalSlowingDistance;
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.CloserDistanceReached);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        curHealthScript = self.GetComponent<BaseEnemy>().healthScript;
        rotSpeed = self.GetComponent<BaseEnemy>().rotationSpeed;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {

    }
}
