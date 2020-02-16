using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SimpleBullet))]
public class Projectile : MonoBehaviour
{

    LineRenderer lineRenderer;
    SpriteRenderer spriteRenderer;
    SimpleBullet simpleBullet;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        simpleBullet = GetComponent<SimpleBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
