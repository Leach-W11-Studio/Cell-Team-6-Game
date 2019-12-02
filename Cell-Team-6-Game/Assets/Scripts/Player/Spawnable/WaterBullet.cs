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

    private float destroyWait = 1;

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ExplodeGroup (List<Collider2D> targetList) {
        foreach (Collider2D collider in targetList) {
            
            HealthScript hs = collider.gameObject.GetComponent<HealthScript>();

            if (!instantKill) {
                hs.TakeDamage(Damage());
            }
            else {
                StartCoroutine(Explode(collider));
            }

            float randomWait = Random.Range(0, explosionTime/targetList.Count);
            destroyWait += randomWait;
            yield return new WaitForSeconds(randomWait);
        }
    }

    public IEnumerator Explode (Collider2D target) {
        Vector3 targetScale = target.transform.localScale * explosionAmount;
        target.GetComponent<HealthScript>().Die(explosionTime);

        for (float elapsedTime = 0; elapsedTime < explosionTime; elapsedTime += Time.deltaTime) {
            float percent = elapsedTime/explosionTime;
            target.transform.localScale = Vector3.Lerp(target.transform.localScale, targetScale, percent);
            yield return new WaitForEndOfFrame();
        }

    }

    protected void OnTriggerEnter2D (Collider2D collision) {
        if (collision.CompareTag("Player") || collision.CompareTag("Untagged")) {
            return;
        }

        Debug.Log(collision.tag);
        List<Collider2D> objects = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, explodeRadius, layerMask));
        for (int i = 0; i < objects.Count; i++) {
            if (!objects[i].CompareTag("Enemy")) {
                objects.Remove(objects[i]);
                i--;
            }
        }
        StartCoroutine(destroy());
        StartCoroutine(ExplodeGroup(objects));
    }

    IEnumerator destroy () {
        rb.isKinematic = true;
        GetComponentInChildren<TrailRenderer>().enabled = false;
        for (float elapsedTime = 0; elapsedTime < destroyWait; elapsedTime += Time.deltaTime){
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }
}
