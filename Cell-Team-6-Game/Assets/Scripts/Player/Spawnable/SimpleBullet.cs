using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour, IShootable
{

    protected int CurrentDamage = 10;
    public float power = 10;
    protected Rigidbody2D rb;

    public Vector3 velocity;
    //ObjectQueue queue;
    private Vector3 lastPosition;

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

    protected void Update() {
        velocity = (transform.position - lastPosition) / Time.deltaTime;

        // Debug.DrawRay(transform.position, velocity, Color.green);
        lastPosition = transform.position;
    }

    public int Damage()
    {
        return CurrentDamage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        /* IDamageable hitObject = collision.GetComponent(typeof(IDamageable)) as IDamageable;
        //Debug.Log(hitObject);
        if (hitObject != null)
        {
            hitObject.takeDamage(CurrentDamage);
        } */

        if(!collision.gameObject.CompareTag("onlyTrigger"))
        {
            gameObject.SetActive(false);
        }
    }
}
