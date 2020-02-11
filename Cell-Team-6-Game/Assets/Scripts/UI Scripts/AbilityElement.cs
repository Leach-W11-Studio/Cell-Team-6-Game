using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AbilityElement : MonoBehaviour
{
    private Sprite abilitySprite;
    private float abilityCooldown;
    private Ability abilityObject;
    private bool currentState;

    public Color OnCooldownColor = Color.gray;

    public void Build(Sprite sprite, float cooldown, Ability ability)
    {
        abilitySprite = sprite;
        abilityCooldown = cooldown;
        abilityObject = ability;

        gameObject.GetComponent<Image>().sprite = abilitySprite;
        currentState = true;
    }

    private void Update()
    {
        if(currentState != abilityObject.castable)
        {
            currentState = abilityObject.castable;
            if(currentState)
            {
                gameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                gameObject.GetComponent<Image>().color = OnCooldownColor;
            }
        }
    }
}
