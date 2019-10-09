using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    //Created necessary Variables
    public Slider Playerhealth;
    public Text Healthtext;
    public int currentHealth;
    public bool sheild = false;
    public bool invincible = false;
    public Color healthColor;
    public Color sheildColor;
    public UnityEvent onTakeDamage;
    private bool isplayer = false;
    private int Damage;

    private Image healthbarFill;

    void Start()
    {
        onTakeDamage = new UnityEvent();
        Playerhealth = FindObjectOfType<Slider>();
        Healthtext = FindObjectOfType<Text>();
        healthbarFill = Playerhealth.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        healthbarFill.color = healthColor;
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

    public void ActivateSheild()
    {
        sheild = true;
        healthbarFill.color = sheildColor;
        Healthtext.text = "SHEILD";
    }

    public void DeactivateSheild()
    {
        sheild = false;
        healthbarFill.color = healthColor;
    }

    //Can be called to restore 1 heart to the player
    public void Restore_Health()
    {
        currentHealth++;
    }

    //Is called to remove one heart from the player
    private void Player_Take_Damage()
    {
        if (invincible) { return; }
        if (sheild) { DeactivateSheild(); onTakeDamage.Invoke(); return; }
        if (currentHealth > 0)
        {
            onTakeDamage.Invoke();
            currentHealth--;
        }
        else
            Die();
    }

    //Is passed a damage value from the collision function, and subtracts the damage from the current health of the enemy
    private void Enemy_Take_Damage(int damageValue)
    {
        if (invincible) { return; }
        if (currentHealth > 0)
        {
            onTakeDamage.Invoke();
            currentHealth -= damageValue;
        }
        else
            Die();
    }

    //On collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If this is attatched to the player, and the object colliding is an enemy or an enemy bullet, subract a heart, and destroy the object that hit it
        if (isplayer && (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Enemy")))
        {
            if (collision.gameObject.CompareTag("Enemy"))
                Destroy(collision.gameObject);
            Player_Take_Damage();
        }
        //If it is not a player, and the collision is a player bullet, pass in the damage value from the bullet and call the enemy damage function
        else if (!isplayer && collision.gameObject.CompareTag("PlayerBullet"))
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
        if (!sheild)
        {
            Healthtext.text = "Health: " + currentHealth;
        }
    }

}
