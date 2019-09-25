using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    //Created necessary Variables
    public Slider Playerhealth;
    public Text Healthtext;
    public int currentHealth;
    private bool isplayer = false;
    private int Damage;

    void Start()
    {
        Playerhealth = FindObjectOfType<Slider>();
        Healthtext = FindObjectOfType<Text>();
        //Tells the script wether to treat the gameobject as a player or an enemy, and set the health accordingly
        if (gameObject.CompareTag("Player"))
        {
            isplayer = true;
            currentHealth = 5;
        }
        else
            currentHealth = 100;
        Playerhealth.maxValue = currentHealth;
    }

    //Can be called to restore 1 heart to the player
    public void Restore_Health()
    {
        currentHealth++;
    }

    //Is called to remove one heart from the player
    private void Player_Take_Damage()
    {
        if (currentHealth > 0)
            currentHealth--;
        else
            Die();
    }

    //Is passed a damage value from the collision function, and subtracts the damage from the current health of the enemy
    private void Enemy_Take_Damage(int damageValue)
    {
        if (currentHealth > 0)
            currentHealth -= damageValue;
        else
            Die();
    }

    //On collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If this is attatched to the player, and the object colliding is an enemy or an enemy bullet, subract a heart, and destroy the object that hit it
        if(isplayer && (collision.gameObject.CompareTag("EnemyBullet")|| collision.gameObject.CompareTag("Enemy")))
        {   
            if(collision.gameObject.CompareTag("Enemy"))
                Destroy(collision.gameObject);
            Player_Take_Damage();
        }
        //If it is not a player, and the collision is a player bullet, pass in the damage value from the bullet and call the enemy damage function
        else if(!isplayer && collision.gameObject.CompareTag("PlayerBullet"))
        {
            Damage = collision.gameObject.GetComponent<SimpleBullet>().Damage();
            Enemy_Take_Damage(Damage);  
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        Playerhealth.value = currentHealth;
        Healthtext.text = "Health: " + currentHealth;
    }

}
