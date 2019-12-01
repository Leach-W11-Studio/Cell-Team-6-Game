using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Sound : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player enters the bounds of the heart, restore health to the player and destroy the heart
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("SND play");
            SoundManager.PlaySound(collision.gameObject.GetHashCode(),"Pickup_Health");
        }
    }
}
