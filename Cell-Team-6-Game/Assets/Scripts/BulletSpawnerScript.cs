using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerScript : MonoBehaviour
{
    public bool shoot;
    public Vector3 force;

    private void FixedUpdate()
    {
        if (!shoot) { return; }
        Shoot();
    }

    public void Shoot() {
        SimpleBullet bullet = ObjectQueue.Instance.SpawnFromPool("Bullet", transform.position, transform.rotation).GetComponent<SimpleBullet>();
        
    }

    
}
