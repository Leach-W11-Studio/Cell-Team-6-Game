using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AbilityElement : MonoBehaviour
{
    private Sprite abilitySprite;
    private Sprite abilityNull;
    private float abilityCooldown;
    private float cooldownCounter;
    private Ability abilityObject;
    private bool currentState;

    public Color OnCooldownColor = Color.gray;

    public void Build(Sprite sprite, Sprite nullSprite, float cooldown, Ability ability)
    {
        abilitySprite = sprite;
        abilityCooldown = cooldown;
        abilityObject = ability;
        abilityNull = nullSprite;

        gameObject.GetComponent<Image>().sprite = abilitySprite;
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = abilityNull;
        gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
    }

    private void Update()
    {
        //gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount -= 1 / cooldownCounter * Time.deltaTime;
        if (abilityObject.castable == false)
        {
            if (gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount <= 0)
            {
                gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            else
            {
                Debug.Log(cooldownCounter);
                Debug.Log(gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount);
                cooldownCounter -= Time.deltaTime;
                gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount -= 1 / abilityCooldown * Time.deltaTime;
            }
        }
        else
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
    }
}
