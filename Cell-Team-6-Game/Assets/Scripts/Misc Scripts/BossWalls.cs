using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthScript))]
public class BossWalls : MonoBehaviour
{
    public int maxHealth = 40;
    public int currentHealth;
    public bool isActive { get; private set; }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet") || other.gameObject.CompareTag("EnemyBullet"))
        {
            var damage = other.gameObject.GetComponent<SimpleBullet>().Damage();
            currentHealth -= damage;
            other.gameObject.SetActive(false);

            if (currentHealth <= 0) { Disable(); }
        }
    }

    public void Enable()
    {
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        isActive = true;
    }

    public void Disable()
    {
        currentHealth = 0;
        gameObject.SetActive(false);
        isActive = false;
    }
}
