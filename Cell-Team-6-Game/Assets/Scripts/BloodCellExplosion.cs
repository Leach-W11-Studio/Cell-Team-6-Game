using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCellExplosion : MonoBehaviour
{
    public float power;
    public float radius;
    public float lifetime;

    new CircleCollider2D collider;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        collider.radius = radius;

        StartCoroutine(Activate());
    }

    IEnumerator Activate() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("DealDamage", power);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
