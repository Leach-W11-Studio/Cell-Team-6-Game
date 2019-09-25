using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerScript : MonoBehaviour
{
    public bool shoot;
    public Vector3 force;
    public float fireRate;

    bool canShoot = true;

    private void FixedUpdate()
    {
        if (!shoot) { return; }
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot() {
        if (canShoot) {
            canShoot = false;
            SimpleBullet bullet = ObjectQueue.Instance.SpawnFromPool("Bullet", transform.position, transform.rotation).GetComponent<SimpleBullet>();
            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }

        yield return null;
    }
}
