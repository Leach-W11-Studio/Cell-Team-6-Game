using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCellPickup : MonoBehaviour
{
    public PlayerInventory.BulletType cellType;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
            inventory.AddBloodCell(cellType, amount);
            Destroy(gameObject);
        }
    }
}
