using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : SimpleBullet
{
    public bool instantKill = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WaterBullet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            HealthScript hs = collision.gameObject.GetComponent<HealthScript>();

            if (instantKill) {
                int damage = hs.currentHealth;
                hs.TakeDamage(damage);
            }
            else {
                hs.TakeDamage(Damage());
            }
        }
    }
}
