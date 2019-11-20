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

    //Used to control how many fire intervals pass between each of these shot types.
    public int megaRateMod;
    public int stunRateMod;

    /// <summary>
    /// The bullet type that is to be actualy fired.
    /// </summary>
    [HideInInspector]
    public string bulletTypeToFire;
    [Tooltip("The name of the bullet in the Object Queue that is to be fired by default")]
    public string standardBulletType = "PlayerBullet";

    public int megaCountDown;
    private int stunCountDown;

    private PlayerInventory inventory;

    #region Depricated Variables
    /*
    //Bools for various gun types
    public bool spreadActive;
    public bool doubleActive;
    public bool megaActive;
    public bool stunActive;
    public bool ricochetActive;
    */
    #endregion

    //public for debug purposes, will be set to private later
    public bool shooting;

    private bool canShoot;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        bulletTypeToFire = standardBulletType;
        megaCountDown = 0;
        stunCountDown = 0;
        inventory = GetComponentInParent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot();
    }

    public void Shoot()
    {
        if (canShoot)
        {
            if (inventory.ContainsBullet(PlayerInventory.BulletType.SpreadShot))
            {
                ObjectQueue.Instance.SpawnFromPool("SpreadBullet", transform.position, transform.rotation);
            }
            if (inventory.ContainsBullet(PlayerInventory.BulletType.DualShot))
            {
                ObjectQueue.Instance.SpawnFromPool("DoubleBullet", transform.position, transform.rotation);
            }
            if (inventory.ContainsBullet(PlayerInventory.BulletType.MegaShot))
            {
                if (megaCountDown == megaRateMod)
                {
                    ObjectQueue.Instance.SpawnFromPool("MegaBullet", transform.position, transform.rotation);
                    megaCountDown = 0;
                }
                megaCountDown += 1;
            }
            if (inventory.ContainsBullet(PlayerInventory.BulletType.StunShot)) { }
            //Depricated due to new ability system
            /* if(inventory.ContainsBullet(PlayerInventory.BulletType.RicochetShot)) {
                ObjectQueue.Instance.SpawnFromPool("RicochetBullet", transform.position, transform.rotation);
            } */

            if (!inventory.ContainsBullet(PlayerInventory.BulletType.DualShot)) { ObjectQueue.Instance.SpawnFromPool(bulletTypeToFire, transform.position, transform.rotation); }

            canShoot = false;
            StartCoroutine(FireRateCheck());
        }
    }

    IEnumerator FireRateCheck()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public void ResetBulletType(float timeDelay) { StartCoroutine(BulletTypeReset(timeDelay)); }
    IEnumerator BulletTypeReset(float timeDelay)
    {
        //Debug.Log("Waiting " + timeDelay + "Seconds");
        yield return new WaitForSeconds(timeDelay);
        //Debug.Log("Waited " + timeDelay + "Seconds");
        bulletTypeToFire = standardBulletType;
    }
}
