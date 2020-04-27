using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : MonoBehaviour, IShootable
{

    public int CurrentDamage = 10;
    public float power = 10;
    public bool spin = true;
    public float maxSpinSpeed = 200;
    protected Rigidbody2D rb;
    public GameObject spriteObject;
    //[Range(0, 1)]
    //public float accuracy = 1;

    public Vector3 velocity;
    private Vector3 direction;
    //ObjectQueue queue;
    private Vector3 lastPosition;
    private float rotationSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        
    }

    void IShootable.Shoot()
    {
        spriteObject = transform.GetChild(0).gameObject;
        //AjustDirection();
        Shoot();
    }

    virtual protected void Shoot() {
        rb.velocity = Vector2.zero;
        rb.AddForce(transform.up * power);

        rotationSpeed = Random.Range(-maxSpinSpeed * 60, maxSpinSpeed * 60);
    }

    //protected void AjustDirection() {
    //    int direction = Random.Range(-1f, 1f) >= 0 ? 1 : -1; // Basically a coin flip. ensures a random choice of 1 or -1;
    //    Vector2 perp = Vector2.Perpendicular(transform.up);
    //    Vector2 randomDirection = Vector2.Lerp(transform.up, direction * perp, accuracy);

    //    transform.up = randomDirection;
    //}

    protected void Update() {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        direction = velocity.normalized;

        if (spin){
            SpinBullet();
        }
        else {
            spriteObject.transform.up = direction;
        }

        // Debug.DrawRay(transform.position, velocity, Color.green);
        lastPosition = transform.position;
    }

    protected void SpinBullet() {
        Vector3 eulers = spriteObject.transform.localEulerAngles;
        spriteObject.transform.localEulerAngles = new Vector3(eulers.x, eulers.y, eulers.z + (rotationSpeed * Time.deltaTime));
    }

    public int Damage()
    {
        return CurrentDamage;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
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
