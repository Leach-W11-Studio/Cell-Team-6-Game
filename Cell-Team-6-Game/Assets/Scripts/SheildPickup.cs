using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildPickup : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            collision.SendMessage("ActivateSheild");
            Destroy(gameObject);
        }
    }
}
