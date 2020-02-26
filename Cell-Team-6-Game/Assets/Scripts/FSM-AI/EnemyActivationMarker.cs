using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivationMarker : MonoBehaviour
{
    public float activationRaidus = 20f;
    public float activationTick = .25f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("WakeEnemies", 0f, activationTick);
    }

    void WakeEnemies()
    {
        LayerMask mask = LayerMask.GetMask("Character");

        var enemiesToWake = Physics2D.OverlapCircleAll(transform.position, activationRaidus * 1.5f, mask);

        foreach (var character in enemiesToWake)
        {
            if(character.gameObject.CompareTag("Enemy"))
            {
                character.gameObject.GetComponent<FSM>().Active = false;
            }
        }

        enemiesToWake = Physics2D.OverlapCircleAll(transform.position, activationRaidus, mask);

        foreach (var character in enemiesToWake)
        {
            if(character.gameObject.CompareTag("Enemy"))
            {
                character.gameObject.GetComponent<FSM>().Active = true;
                character.gameObject.GetComponent<FSM>().enemyAnim.SetTrigger("IsActive");
            }
        }
    }
}
