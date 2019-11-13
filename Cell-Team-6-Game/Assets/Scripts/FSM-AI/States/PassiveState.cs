using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveState : FSMState
{
    private float activateRaidus;
    private HealthScript curHealthScript;

    public PassiveState(float callRaidus)
    {
        stateID = FSMStateID.Passive;
        activateRaidus = callRaidus;
    }

    public override void Act(Transform player, GameObject self)
    {
        //Doesn't do anything
    }

    public override void Reason(Transform player, GameObject self)
    {
        if (curHealthScript.currentHealth != curHealthScript.maxHealth)
        {
            self.GetComponent<BaseEnemy>().isAwake = true;

            LayerMask charMask = LayerMask.GetMask("Character");
            if (activateRaidus > 0)
            {
                var objectsInRad = Physics2D.OverlapCircleAll(self.transform.position, activateRaidus, charMask);
                foreach (var item in objectsInRad)
                {
                    if (item.CompareTag("Enemy"))
                    {
                        item.GetComponent<BaseEnemy>().isAwake = true;
                    }
                }
            }

            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.Awoken);
        }
        else if (self.GetComponent<BaseEnemy>().isAwake)
        {
            self.GetComponent<BaseEnemy>().SetTransition(FSMTransitions.Awoken);
        }
    }

    public override void OnStateEnter(Transform player, GameObject self)
    {
        curHealthScript = self.GetComponent<BaseEnemy>().healthScript;
    }

    public override void OnStateExit(Transform player, GameObject self)
    {

    }
}
