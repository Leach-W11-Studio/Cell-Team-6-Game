using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : SimpleBullet
{
    public LayerMask layers;
    public int maxBounces = 0;
    int bounces = 0;

    private string oppositeTag {
        get {
            if (gameObject.CompareTag("PlayerBullet")) { return "EnemyBullet"; }
            else { return "PlayerBullet"; }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(oppositeTag))
        {
            if (bounces < maxBounces)
            {
                Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.blue);
                Bounce(collision.contacts[0].normal);
            }
            else {
                gameObject.SetActive(false);
            }
        }
        else {
            IDamageable hitObject = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
            //Debug.Log(hitObject);
            if (hitObject != null)
            {
                hitObject.takeDamage(CurrentDamage);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        bounces = 0;
    }

    private void Bounce(Vector2 surfaceNormal)
    {
        transform.up = Vector2.Reflect(velocity.normalized, surfaceNormal);
        // Debug.DrawRay(transform.position, transform.up, Color.yellow);
        Shoot();
        bounces++;
    }
}
