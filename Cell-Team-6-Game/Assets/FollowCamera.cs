using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float acceleration;
    public float lookAhead;
    public bool followPlayer;

    public Transform target;

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
    }

    void Move() {
        if (followPlayer && !target) { target = FindPlayer(); }
        if (!target) { Debug.LogError("Could not locate target!"); return; }

        Vector2 targetPos = target.position;
        Rigidbody2D targetRB = target.GetComponent<Rigidbody2D>();
        if (targetRB) {
            Vector2 direction = targetRB.velocity.normalized;
            targetPos = targetPos + (direction * lookAhead);
        }

        Debug.DrawLine(target.position, targetPos, Color.yellow);

        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, targetPos.y, transform.position.z), Time.fixedDeltaTime * acceleration);
    }

    public Transform FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player.transform;
    }
}
