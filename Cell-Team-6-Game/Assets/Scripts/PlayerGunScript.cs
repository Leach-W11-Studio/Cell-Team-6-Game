using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunScript : MonoBehaviour
{
    /// <summary>
    /// Describes the delay between shots of the player's weapon.
    /// </summary>
    [Tooltip("Describes the delay between shots of the player's weapon.")]
    public float fireRate;

    //Bools for various gun types
    public bool spreadActive;
    public bool doubleActive;
    public bool megaActive;
    public bool stunActive;
    public bool ricochetActive;

    //public for debug purposes, will be set to private later
    public bool shooting;

    private bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot();
    }

    public void Shoot()
    {
        if(canShoot)
        {
            if(spreadActive) { ObjectQueue.Instance.SpawnFromPool("SpreadBullet", transform.position, transform.rotation); }
            if(doubleActive) { ObjectQueue.Instance.SpawnFromPool("DoubleBullet", transform.position, transform.rotation); }
            if(megaActive) { }
            if(stunActive) { }
            if(ricochetActive) { }

            if(!doubleActive) { ObjectQueue.Instance.SpawnFromPool("PlayerBullet", transform.position, transform.rotation); }

            canShoot = false;
            StartCoroutine(FireRateCheck());
        }
    }

    IEnumerator FireRateCheck()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
