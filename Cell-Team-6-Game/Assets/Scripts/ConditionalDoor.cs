using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalDoor : DoorScript
{
    public UnityEvent onEnemiesDefeated;
    public UnityEvent onEnemiesChanged;
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    public bool playerInArea = false;
    public bool defeated = false;
    public bool locked = false;
    private PolygonCollider2D conditionalArea;
    private bool playerWasInArea = false;

    private void Start()
    {
        base.Start();
        conditionalArea = transform.Find("Condition Area").GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (!playerWasInArea && playerInArea)
        {
            onPlayerEnter.Invoke();
        }
        else if (playerWasInArea && !playerInArea) {
            onPlayerExit.Invoke();
        }

        playerWasInArea = playerInArea;
    }

    public override void Open() {
        if (!locked) {
            base.Open();
        }
    }

    public override void Close() {
        if (!defeated) {
            locked = true;
            base.Close();
        }
        else {
            base.Close();
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
