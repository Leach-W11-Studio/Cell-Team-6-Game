using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All abilities must inherit from Ability and impliment a constructor with the ability name, and the two abstract classes below.
public class RiotShieldAbility : Ability
{
    private GameObject RiotShield;
    private GameObject RiotShieldUI;
    public RiotShieldAbility()
    {
        abilityName = "RiotShield";
        RiotShield = GameObject.Find("RiotShield");
        RiotShieldUI = GameObject.Find("RiotshieldUI");
    }

    public override void OnPickup()
    {
        //All functionality to be activated imidately on pickup of this ability to be placed in here
        Debug.Log("OnPickup called on ability: " + abilityName);
        RiotShieldUI.SetActive(true);
    }

    protected override void CastAction()
    {
        //All actions to be taken upon a successful cast of this ability are to be placed here
        Debug.Log("CastAction called on ability: " + abilityName);
        RiotShield.SetActive(true);
    }

    protected override bool CastCondition()
    {
        //Any checks needed to be cast are placed here, return true if the conditions are met, false otherwise.
        if (Input.GetKey(KeyCode.U)) { return false; }
        else { return true; }
    }
}