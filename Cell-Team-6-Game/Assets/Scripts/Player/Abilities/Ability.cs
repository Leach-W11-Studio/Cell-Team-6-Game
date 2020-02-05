﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    protected string abilityName;
    public string AbilityName { get { return abilityName; } }

    public float abilityCooldown;
    public float AbilityCooldown { get { return abilityCooldown; } }

    public bool castable;

    public Image abilityIcon;

    private void Start()
    {
        castable = true;
    }
    protected virtual bool CastCondition() { return true; }
    public virtual void Cast()
    {
        if (castable && CastCondition())
        {
            Debug.Log("Casting Ability: " + abilityName);

            castable = false;
            CastAction();
            StartCoroutine(resetCooldown());
        }
    }
    protected abstract void CastAction();
    public abstract void OnPickup();

    IEnumerator resetCooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);
        castable = true;
    }
}
