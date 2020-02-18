using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntersMark : Ability
{
    public HuntersMark()
    {
        abilityName = "HuntersMark";
    }

    private GameObject[] enemies;
    private GameObject closestEnemy;

    public float maximumMarkDistance = 20;

    public override void OnPickup()
    {
        //Nothing
    }

    protected override void CastAction()
    {
        
    }

    protected override bool CastCondition()
    {
        closestEnemy = null;

        float currentMinDistance = maximumMarkDistance;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float curDistance = Vector2.Distance(enemy.transform.position, transform.position);
            if (curDistance < currentMinDistance)
            {
                closestEnemy = enemy;
                currentMinDistance = curDistance;
            }
        }

        if (closestEnemy) { return true; }
        else { return false; }
    }
}
