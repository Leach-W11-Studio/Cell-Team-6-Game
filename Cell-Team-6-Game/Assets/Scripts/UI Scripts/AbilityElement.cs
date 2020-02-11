using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AbilityElement : MonoBehaviour
{
    private Sprite abilitySprite;
    private float abilityCooldown;

    public void Build(Sprite sprite, float cooldown)
    {
        abilitySprite = sprite;
        abilityCooldown = cooldown;

        gameObject.GetComponent<Image>().sprite = abilitySprite;
    }


    public void SetVisualCooldown(float seconds)
    {
        //Implimentation
    }
}
