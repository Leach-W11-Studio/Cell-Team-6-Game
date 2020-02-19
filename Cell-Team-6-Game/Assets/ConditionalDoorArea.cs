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
    // Start is called before the first frame update
    void Start()
    {
        door = transform.parent.GetComponent<ConditionalDoor>();
        myCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        enemies.Clear();
        List<Collider2D> colliders = new List<Collider2D>(); 
        myCollider.OverlapCollider(contactFilter, colliders);
        foreach (Collider2D collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                enemies.Add(collider);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            playerInArea = false;
        }
    }
}
