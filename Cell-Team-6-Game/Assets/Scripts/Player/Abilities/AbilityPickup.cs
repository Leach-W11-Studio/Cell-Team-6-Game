using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public GameObject abilityObject;

    // Start is called before the first frame update
    void Start()
    {
        if (!abilityObject.GetComponent<Ability>())
        {
            Debug.LogError("Ability not present on object");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            AbilityCaster caster = other.GetComponentInChildren<AbilityCaster>();
            caster.AddAbility(abilityObject);
            Destroy(gameObject);
        }
    }
}
