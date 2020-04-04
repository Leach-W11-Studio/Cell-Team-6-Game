using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class ConditionalDoorArea : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    private bool playerInArea;
    public List<Collider2D> enemies;
    private ConditionalDoor door;
    private PolygonCollider2D myCollider;

    public List<Collider2D> oldEnemies;
    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.GetComponent<ConditionalDoor>();
        myCollider = GetComponent<PolygonCollider2D>();
        GetEnemies();
        oldEnemies = new List<Collider2D>(enemies);
    }

    // Update is called once per frame
    void Update()
    {
        GetEnemies();
        foreach (var enemy in enemies) {
            if (enemy == null) {
                enemies.Remove(enemy);
            }
        }

        if (enemies.Count == 0) {
            door.Defeat();
        }

    }

    private void GetEnemies() {
        bool start = false;
        if (enemies.Count == 0) {
            start = true;
        }
        oldEnemies = new List<Collider2D>(enemies);
        enemies.Clear();
        List<Collider2D> colliders = new List<Collider2D>(); 
        Physics2D.OverlapCollider(myCollider, contactFilter, colliders);
        foreach (Collider2D collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                enemies.Add(collider);
            }
        }

        if (enemies.Count != oldEnemies.Count && !start) {
            door.onEnemiesChanged.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!door.defeated)
            {
                door.playerInArea = true;
            }
            else {
                door.playerInArea = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (!door.defeated) { 
                door.playerInArea = false;
            }
        }
    }
}
