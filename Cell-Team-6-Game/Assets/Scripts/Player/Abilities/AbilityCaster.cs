using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    public List<Ability> activeAbilities = new List<Ability>();
    public float maxAbilities;
    private GameObject abilityContainer;
    private VariableAbilityUI abilityUI;

    private void Start()
    {
        if (maxAbilities > 5)
        {
            Debug.LogError("maxAbilities should not be set to greater than 5");
            maxAbilities = 5;
        }
        abilityContainer = gameObject;
        abilityUI = GameObject.Find("AbilityUIPanel").GetComponent<VariableAbilityUI>();
    }
    private void Update()
    {
        UpdateAbilityCasts();
    }

    public void AddAbility(GameObject abilityObject)
    {
        if (activeAbilities.Count < maxAbilities)
        {
            //if (activeAbilities.Contains(abilityObject.GetComponent<Ability>())) { return; }

            GameObject newAbility = Instantiate(abilityObject);
            newAbility.transform.SetParent(abilityContainer.transform);

            newAbility.GetComponent<Ability>().OnPickup();
            activeAbilities.Add(newAbility.GetComponent<Ability>());

            abilityUI.UpdateAbilityUI(activeAbilities);
        }
        else
        {
            Debug.Log("Max abilities reached, ability was not added");
        }
    }

    //Note that two abilities can't be cast on the same update window. If two buttons are pressed at the same time, funciton defaults to lowest numbered ability
    private void UpdateAbilityCasts()
    {
        if (Input.GetAxis("Cast1") != 0 && activeAbilities.Count >= 1)
        {
            activeAbilities[0].Cast();
        }
        else if (Input.GetAxis("Cast2") != 0 && activeAbilities.Count >= 2)
        {
            activeAbilities[1].Cast();
        }
        else if (Input.GetAxis("Cast3") != 0 && activeAbilities.Count >= 3)
        {
            activeAbilities[2].Cast();
        }
        else if (Input.GetAxis("Cast4") != 0 && activeAbilities.Count >= 4)
        {
            activeAbilities[3].Cast();
        }
        else if (Input.GetAxis("Cast5") != 0 && activeAbilities.Count >= 5)
        {
            activeAbilities[4].Cast();
        }
    }
}
