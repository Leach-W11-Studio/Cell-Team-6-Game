using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Sound : MonoBehaviour
{
    
        // Start is called before the first frame update
    void Start()
    {
        
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.PlaySound(gameObject.GetHashCode(), "PickUp_Health");
        }
    }
}
