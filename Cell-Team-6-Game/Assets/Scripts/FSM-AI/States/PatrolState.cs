﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : FSMState
{
    private List<GameObject> PatrolPoints;
    private Vector3 DestPos;
    private PolyNavAgent agent;

    public PatrolState(List<GameObject> points, PolyNavAgent navAgent)
    {
        stateID = FSMStateID.Patrol;
        PatrolPoints = points;
        agent = navAgent;
        DestPos = SelectPatrolPoint();
    }

    public override void Act(Transform player, GameObject self)
    {
        if(agent.remainingDistance <= .1f)
        {
            DestPos = SelectPatrolPoint();
            agent.SetDestination(DestPos);
        }

        Vector2 heading = DestPos - self.transform.position;
        heading.Normalize();
        float zRot = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        self.transform.rotation = Quaternion.Lerp(self.transform.rotation, Quaternion.Euler(0f, 0f, zRot), Time.fixedDeltaTime * self.GetComponent<BaseEnemy>().rotationSpeed);
    }

    private Vector3 SelectPatrolPoint()
    {
        var selectedPoint = PatrolPoints[Random.Range(0, PatrolPoints.Count)];
        return selectedPoint.transform.position;
    }

    public override void Reason(Transform player, GameObject self)
    {
        if(self.GetComponent<BaseEnemy>().healthScript.currentHealth <= 0)
        {
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.OutOfHealth);
        }
        else if(Vector2.Distance(self.transform.position, player.position) <= self.GetComponent<BaseEnemy>().agroDistance)
        {
            agent.Stop();
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.SawPlayer);
        }
    }
}
