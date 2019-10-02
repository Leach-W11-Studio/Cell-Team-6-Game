using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    private HealthScript Shield;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player enters the bounds of the heart, restore health to the player and destroy the heart
        if (collision.gameObject.CompareTag("Player"))
        {
            Shield = collision.gameObject.GetComponent<HealthScript>();
            Shield.Shield_Player();
            Destroy(gameObject);
        }
    }
}
