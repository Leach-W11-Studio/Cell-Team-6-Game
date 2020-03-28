using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletPhase2 : SimpleBullet
{

    public float rotationSpeed;
    public float maxDistance;

    private Vector2 startingPos;
    private float variedRotationSpeed;
    private Transform player;

    override protected void Shoot() {
        startingPos = transform.position;
        //variedRotationSpeed = Random.Range(rotationSpeed * 0.2f, rotationSpeed);
        variedRotationSpeed = rotationSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        base.Shoot();
    }

    // Start is called before the first frame update
    void Start()
    {
        Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (enabled)
        {
            if (Vector2.Distance(transform.position, startingPos) > maxDistance)
            {
                gameObject.SetActive(false);
            }

            if (player) {
                Aim();
            }
        }

    }

    void Aim() {
        Vector3 heading = player.position - transform.position;
        Vector3 direction = heading.normalized;
        Debug.DrawRay(transform.position, direction, Color.yellow);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * variedRotationSpeed);
        transform.rotation = newRotation;
        rb.velocity = transform.up * rb.velocity.magnitude;
        Debug.DrawRay(transform.position, velocity, Color.blue);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, transform.up * maxDistance, Color.red);
        if (rotationSpeed > 0)
        {
            float radius = power / rotationSpeed;
            if (Application.isPlaying)
            {
                radius = power / variedRotationSpeed;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
