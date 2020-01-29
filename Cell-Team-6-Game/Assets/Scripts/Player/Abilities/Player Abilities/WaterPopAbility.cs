using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPopAbility : Ability
{
    PlayerGunScript gunScript;

    // Start is called before the first frame update
    void Start()
    {
        gunScript = FindObjectOfType<PlayerGunScript>();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void CastAction() {
        Debug.Log("should shoot water bullet");
        gunScript.Shoot("WaterBullet");
    }

    public override void OnPickup() {

    }
}
