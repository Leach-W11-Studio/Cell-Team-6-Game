using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : SimpleBullet
{
    public bool instantKill = false;
    public float explodeRadius;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WaterBullet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ExplodeGroup(List<Collider2D> targetList) {

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

            List<Collider2D> objects = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, explodeRadius, layerMask));


            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Environment")) {
            gameObject.SetActive(false);
        }
    }
}
