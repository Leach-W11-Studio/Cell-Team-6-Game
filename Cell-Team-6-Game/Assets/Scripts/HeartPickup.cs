using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    private HealthScript health;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player enters the bounds of the heart, restore health to the player and destroy the heart
        if (collision.gameObject.CompareTag("Player"))
        {
            health = collision.gameObject.GetComponent<HealthScript>();
            health.Restore_Health();
            Destroy(gameObject);
        }
    }
}
