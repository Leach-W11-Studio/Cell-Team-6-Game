﻿using System.Collections;
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
    private Vector2 offset;
    //private Vector2 shieldpos;
    //private Vector2 playerpos;
    //private GameObject mainplayer;

    private void OnEnable()
    {
        StartCoroutine("ShieldTimer");
    }

    private void Start()
    {
        //gameObject.SetActive(false)
        //mainplayer = GameObject.FindGameObjectWithTag("Player");
        //shieldpos = new Vector2(transform.position.x, transform.position.y);
        //playerpos = new Vector2(mainplayer.transform.position.x, mainplayer.transform.position.y);
        //Needed in order to get the power value of the bullet that we are using
        playbull = playerBullet.GetComponent<SimpleBullet>();
        //offset = shieldpos - playerpos;
    }

    private void Update()
    {
        //playerpos = new Vector2(mainplayer.transform.position.x, mainplayer.transform.position.y);
        //transform.position = playerpos + offset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Finding the components of the bullet that collided
        collidedBullet = collision.gameObject;
        rb = collidedBullet.GetComponent<Rigidbody2D>();

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("should ricochet");
            //Used adams racast ricocchet script to get the surface angle
            RaycastHit2D surface = Physics2D.Raycast(collidedBullet.transform.localPosition, rb.velocity.normalized, layers);

            //Set a variable to the up direction of the bullet that collided
            bulletTrans = collidedBullet.transform.up;

            //Sets an instanitation to instantiate a bullet in the inverse direction that the bullet came from
            RicochetBullet = Instantiate(playerBullet, collidedBullet.transform.localPosition, Quaternion.identity);

            RicochetBullet.transform.up = Vector2.Reflect(bulletTrans, surface.normal);
            //Instantiates a player bullet and adds force based on its power value
            RicochetBullet.GetComponent<Rigidbody2D>().AddForce(RicochetBullet.transform.up * playbull.power);

            collidedBullet.SetActive(false);
        }
    }

    IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(Countdown);
        gameObject.SetActive(false);
    }
}
