using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiotShield : MonoBehaviour
{
    public LayerMask layers;
    private SimpleBullet playbull;
    private GameObject RicochetBullet;
    public GameObject playerBullet;
    private Vector2 bulletTrans;
    private Rigidbody2D rb;
    private GameObject collidedBullet;
    public float Countdown = 5f;

    private void Start()
    {
        //Needed in order to get the power value of the bullet that we are using
        playbull = playerBullet.GetComponent<SimpleBullet>();
        StartCoroutine("ShieldTimer");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Finding the components of the bullet that collided
        collidedBullet = collision.gameObject;
        rb = collidedBullet.GetComponent<Rigidbody2D>();

        if (collision.CompareTag("EnemyBullet"))
        {
            Debug.Log("should ricochet");
            //Used adams racast ricocchet script to get the surface angle
            RaycastHit2D surface = Physics2D.Raycast(collidedBullet.transform.position, rb.velocity.normalized,layers);

            //Set a variable to the up direction of the bullet that collided
            bulletTrans = collidedBullet.transform.up;

            collidedBullet.SetActive(false);

            //Sets an instanitation to instantiate a bullet in the inverse direction that the bullet came from
            RicochetBullet = Instantiate(playerBullet, Vector2.Reflect(bulletTrans, surface.normal), Quaternion.identity);

            //Instantiates a player bullet and adds force based on its power value
            RicochetBullet.GetComponent<Rigidbody2D>().AddForce(transform.up * playbull.power);

        }
    }

    IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(Countdown);
        gameObject.SetActive(false);
    }
}
