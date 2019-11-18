using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    private List<GameObject> PatrolPoints;
    private Vector3 DestPos;
    private PolyNavAgent agent;
    private HealthScript selfHealthScript;
    private float rotationSpeed;
    private float agroDistance;

    public PatrolState(List<GameObject> points, PolyNavAgent navAgent, float agroRange)
    {
        stateID = FSMStateID.Patrol;
        if (points.Count == 0)
        {
            Debug.LogError("No patrol points are listed. Add patrol points to this enemy in the inspector");
        }
        PatrolPoints = points;
        agent = navAgent;
        DestPos = SelectPatrolPoint();
        agroDistance = agroRange;
    }

    public override void Act(Transform player, GameObject self)
    {
        if (agent.remainingDistance <= .1f)
        {
            DestPos = SelectPatrolPoint();
            agent.SetDestination(DestPos);
        }

        Vector2 heading = DestPos - self.transform.position;
        heading.Normalize();
        float zRot = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        self.transform.rotation = Quaternion.Lerp(self.transform.rotation, Quaternion.Euler(0f, 0f, zRot), Time.fixedDeltaTime * rotationSpeed);
    }

    private Vector3 SelectPatrolPoint()
    {
        var selectedPoint = PatrolPoints[Random.Range(0, PatrolPoints.Count)];
        return selectedPoint.transform.position;
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (selfHealthScript.currentHealth <= 0)
        {
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        else if (Vector2.Distance(self.transform.position, player.position) <= agroDistance)
        {
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.SawPlayer);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        selfHealthScript = self.GetComponent<BaseEnemy>().healthScript;
        rotationSpeed = self.GetComponent<BaseEnemy>().rotationSpeed;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {

    }
}
