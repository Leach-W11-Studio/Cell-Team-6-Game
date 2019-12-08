using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructAbility : Ability
{
    public float radius;
    public LayerMask layerMask;
    public int damage;
    public int selfDamage;
    public float force;
    
    PlayerController player;
    HealthScript playerHealth;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerHealth = player.GetComponent<HealthScript>();
        animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override bool CastCondition() {
        Debug.Log("Checking to cast self destruct");
        if (playerHealth.currentHealth > selfDamage) {
            return true;
        }
        else {
            return false;
        }
    }

    protected override void CastAction() {
        Debug.Log("Casting self destruct");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        animator.SetTrigger("selfDestruct");
        player.enabled = false;
        playerHealth.TakeDamage(selfDamage);
        playerHealth.invincible = true;
        StartCoroutine(PlayerControlDelay());
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.CompareTag("Enemy")) {
                HealthScript enemyHealth = collider.gameObject.GetComponent<HealthScript>();
                PolyNavAgent agent = collider.gameObject.GetComponentInChildren<PolyNavAgent>();
                Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
                if (enemyHealth) {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log("Enemy Taking Damage");
                }
                if (agent) {
                    agent.enabled = false;
                    StartCoroutine(EnemyPathfindDelay(agent));
                }
                if (rb) {
                    Vector2 forceVector = (transform.position - collider.gameObject.transform.position).normalized;
                    rb.AddForce(forceVector * force);
                }
            }
        }
    }

    IEnumerator EnemyPathfindDelay (PolyNavAgent agent) {
        yield return new WaitForSeconds(1);
        agent.enabled = true;
    }

    IEnumerator PlayerControlDelay () {
        yield return new WaitForSeconds(1);
        playerHealth.invincible = false;
        player.enabled = true;
    }

    public override void OnPickup() {

    }
}
