using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalDoor : DoorScript
{
    public UnityEvent onEnemiesDefeated;
    public UnityEvent onEnemiesChanged;
    public bool playerInArea;
    public bool defeated = false;
    public bool locked = false;
    private PolygonCollider2D conditionalArea;
    private void Start()
    {
        conditionalArea = transform.Find("Condition Area").GetComponent<PolygonCollider2D>();
    }

    public override void Open() {
        if (!locked) {
            base.Open();
        }
    }

    public override void Close() {
        if (!defeated) {
            locked = true;
        }
    }
    public virtual void Defeat() {
        if (!defeated) {
            defeated = true;
            onEnemiesDefeated.Invoke();
        }
    }

    public virtual void Lock () {locked = true;}
    public virtual void Unlock () {locked = false;}
}
