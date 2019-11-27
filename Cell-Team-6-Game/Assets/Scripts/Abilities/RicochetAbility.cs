using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetAbility : Ability
{
    private PlayerGunScript gunScript;
    public string RicochetBulletName = "RicochetBullet";
    public float abilityDuration = 3f;

    public RicochetAbility()
    {
        abilityName = "Ricochet";
    }

    // Start is called before the first frame update
    public override void OnPickup()
    {
        gunScript = GameObject.Find("Gun").GetComponent<PlayerGunScript>();
    }

    protected override void CastAction()
    {
        gunScript.bulletTypeToFire = RicochetBulletName;
        gunScript.ResetBulletType(abilityDuration);
    }
}
