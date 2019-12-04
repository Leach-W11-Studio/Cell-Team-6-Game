using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseHeadAbility : Ability
{
    public GameObject horseHeadPrefab;
    public bool SetPlayerSpriteInactive = false;
    private GameObject player;
    private bool horseHeadIsActive;
    private GameObject playerSprite;
    private GameObject horseSprite;

    public HorseHeadAbility()
    {
        abilityName = "HorseHead";
    }

    public override void OnPickup()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSprite = player.transform.Find("PlayerSprite").gameObject;

        if (horseHeadPrefab == null)
        {
            Debug.LogError("HorseHead not assigned");
            return;
        }
        if (SetPlayerSpriteInactive)
        {
            playerSprite.SetActive(false);
        }

        horseSprite = Instantiate(horseHeadPrefab);
        horseSprite.transform.parent = player.transform;
        horseSprite.transform.localPosition = Vector3.zero;
        horseSprite.transform.localRotation = Quaternion.Euler(0, 0, 180);

        horseHeadIsActive = true;
    }

    protected override void CastAction()
    {
        //throw new System.NotImplementedException();
        horseHeadIsActive = !horseHeadIsActive;
        ToggleHorseHead(horseHeadIsActive);
    }

    private void ToggleHorseHead(bool active)
    {
        horseSprite.SetActive(active);
        if (SetPlayerSpriteInactive) { playerSprite.SetActive(!active); }
    }
}
