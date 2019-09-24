using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    public float power = 10;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot() {
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.right * power);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //gameObject.SetActive(false);
    }
}
