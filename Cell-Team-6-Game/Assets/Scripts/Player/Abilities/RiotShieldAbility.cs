using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All abilities must inherit from Ability and impliment a constructor with the ability name, and the two abstract classes below.
public class RiotShieldAbility : Ability
{
    public GameObject RiotShield;
    public GameObject RiotChild;
    private GameObject RiotShieldObject;
    private GameObject RiotShieldUI;
    private GameObject player;

    public RiotShieldAbility()
    {
        abilityName = "RiotShield";
    }

    public override void OnPickup()
    { 
        //All functionality to be activated imidately on pickup of this ability to be placed in here
        Debug.Log("OnPickup called on ability: " + abilityName);
        player = GameObject.FindWithTag("Player");

        RiotShieldObject = Instantiate(RiotShield, player.transform.position, player.transform.rotation);
        //RiotShieldObject.transform.SetParent(player.transform);
        RiotChild = RiotShieldObject.transform.GetChild(0).gameObject;
        RiotChild.SetActive(false);
        //RiotShieldUI.SetActive(true);
    }

    protected override void CastAction()
    {
        //All actions to be taken upon a successful cast of this ability are to be placed here
        Debug.Log("CastAction called on ability: " + abilityName);
        RiotChild.SetActive(true);
    }

    protected override bool CastCondition()
    {
        //Any checks needed to be cast are placed here, return true if the conditions are met, false otherwise.
        //if (Input.GetKey(KeyCode.U)) { return false; }
        return true;
    }
}