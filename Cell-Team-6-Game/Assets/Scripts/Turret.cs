using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform muzzle;
    public BulletSpawnerScript spawner;
    public Transform player;

    public float rotationSpeed = 5f;

    private void Awake()
    {
        muzzle = transform.Find("Muzzle");
        spawner = muzzle.GetComponent<BulletSpawnerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Try to find the player if not yet found.
        FindPlayer();

        // If the player is within range, Aim and shoot
        Aim();
    }

    void Aim() {
        if (!player) { return; }
        Vector2 heading = player.position - transform.position;
        heading.Normalize();
        float zRot = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, zRot), Time.fixedDeltaTime * rotationSpeed);
    }

    void FindPlayer()
    {
        if (player) { return; }

        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            spawner.shoot = true;
        }
        catch (Exception e) {
            Debug.LogWarning("Could not find player...");
            spawner.shoot = false;
        }
    }
}
