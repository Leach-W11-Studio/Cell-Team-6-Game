using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour, IShootable
{

    private int CurrentDamage = 10;
    public float power = 10;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void IShootable.Shoot()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * power);
    }

    public int Damage()
    {
        return CurrentDamage;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
