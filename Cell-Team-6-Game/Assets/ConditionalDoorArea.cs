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
        List<Collider2D> oldEnemies = enemies;
        enemies.Clear();
        List<Collider2D> colliders = new List<Collider2D>(); 
        Physics2D.OverlapCollider(myCollider, contactFilter, colliders);
        foreach (Collider2D collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                enemies.Add(collider);
            }
        }

        if (enemies != oldEnemies) {
            door.onEnemiesChanged.Invoke();
            Debug.Log("enemies changed");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            door.playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            door.playerInArea = false;
        }
    }
}
