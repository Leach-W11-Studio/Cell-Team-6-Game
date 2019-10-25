﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : SimpleBullet
{
    public LayerMask layers;
    public int maxBounces = 0;
    int bounces = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Environment"))
        {
            Debug.Log("bounces is: " + bounces + " and maxBounces is: " + maxBounces);
            if (bounces < maxBounces)
            {
                Debug.Log("should ricochet");
                RaycastHit2D surface = Physics2D.Linecast(transform.position, collision.transform.position, layers);
                Debug.DrawRay(surface.point, surface.normal, Color.blue);
                Bounce(surface.normal);
            }
            else {
                gameObject.SetActive(false);
            }
        }
        else {
            IDamageable hitObject = collision.GetComponent(typeof(IDamageable)) as IDamageable;
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
        transform.up = Vector2.Reflect(transform.up, surfaceNormal);
        Debug.DrawRay(transform.position, transform.up, Color.yellow);
        GetComponent<IShootable>().Shoot();
        bounces++;
    }
}