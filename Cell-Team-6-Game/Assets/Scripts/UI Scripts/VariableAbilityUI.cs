using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableAbilityUI : MonoBehaviour
{
    public GameObject abilityUIPanel;
    public GameObject basicAbilityElement;

    private List<AbilityElement> abilityElements = new List<AbilityElement>();

    // Start is called before the first frame update
    void Start()
    {
        abilityUIPanel = this.gameObject;
    }

    public void UpdateAbilityUI(List<Ability> abilities)
    {
        int counter = 0;
        ClearAbilityDisplay();
        foreach (var ability in abilities)
        {
            AddAbilityElement(ability);
            counter++;
        }
        Debug.Log(counter + "Abilities have been generated on the display");
    }

    private void AddAbilityElement(Ability toAdd)
    {
        var addedAbility = Instantiate(basicAbilityElement);
        addedAbility.transform.SetParent(gameObject.transform);
        var tempElement = addedAbility.GetComponent<AbilityElement>();

        abilityElements.Add(tempElement);
        tempElement.Build(toAdd.abilityIcon, toAdd.abilityNull, toAdd.abilityCooldown, toAdd);
    }

    private void ClearAbilityDisplay()
    {
        if (abilityElements.Count > 0)
        {
            foreach (Transform child in abilityUIPanel.transform)
            {
                Destroy(child.gameObject);
            }
            abilityElements.Clear();
        }
    }
}
