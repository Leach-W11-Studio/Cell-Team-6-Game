using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{
    //Created necessary Variables
    public Text Healthtext;
    public int maxHealth;
    public int currentHealth;
    public bool sheild;
    public bool invincible = false;
    public Color healthColor;
    public Color sheildColor;
    public UnityEvent onTakeDamage;
    public bool isplayer = false; //Testing Remove Later
    private int Damage;
    private int Deathtime;
    private Animator PlayerAnim;

    void Start()
    {
        PlayerAnim = gameObject.GetComponent<Animator>();
        onTakeDamage = new UnityEvent();
        Deathtime = 1;
        //currentHealth = maxHealth;
        if (transform.tag == "Player") { isplayer = true; }

        if (!isplayer) { currentHealth = maxHealth; }
    }

    public void ActivateSheild()
    {
        sheild = true;
        Healthtext.text = "SHEILD";
    }

    public void DeactivateSheild()
    {
        sheild = false;
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
        if (currentHealth == 0) { Die(); }
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
        //if (collision.CompareTag("PlayerBullet")) { Debug.Log("Bonk"); }
        //If this is attatched to the player, and the object colliding is an enemy or an enemy bullet, subract a heart, and destroy the object that hit it
        if (isplayer && (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Enemy")))
        {
            //Debug.Log("being hit");
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
        if (isplayer) { PlayerAnim.SetTrigger("died"); }
        StartCoroutine("DieWait");
    }

    IEnumerator DieWait()
    {
        yield return new WaitForSeconds(Deathtime);
        Destroy(gameObject, .1f);
    }
}
