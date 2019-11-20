using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : SimpleBullet
{
    public bool instantKill = false;
    public float explodeRadius;
    public float explosionTime;
    public float explosionAmount = 2;
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

    private IEnumerator ExplodeGroup (List<Collider2D> targetList) {
        foreach (Collider2D collider in targetList) {
            StartCoroutine(Explode(collider));
            yield return new WaitForSeconds(Random.Range(0, explosionTime/targetList.Count));
        }
    }

    private IEnumerator Explode (Collider2D target) {
        Vector3 targetScale = target.transform.localScale*explosionAmount;
        for (float elapsedTime = 0; elapsedTime < explosionTime; elapsedTime += Time.deltaTime) {
            float percent = elapsedTime/explosionTime;
            target.transform.localScale = Vector3.Lerp(target.transform.localScale, targetScale, percent);
            yield return new WaitForEndOfFrame();
        }

        target.GetComponent<HealthScript>().Die();
    }

    protected void OnTriggerEnter2D (Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            HealthScript hs = collision.gameObject.GetComponent<HealthScript>();

            List<Collider2D> objects = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, explodeRadius, layerMask));
            for (int i = 0; i < objects.Count; i++) {
                if (!objects[i].CompareTag("Enemy")) {
                    objects.Remove(objects[i]);
                }
            }

            if (instantKill) {
                StartCoroutine(ExplodeGroup(objects));
            }
            else {
                hs.TakeDamage(Damage());
            }

            gameObject.SetActive(false);
        }
    }
}
