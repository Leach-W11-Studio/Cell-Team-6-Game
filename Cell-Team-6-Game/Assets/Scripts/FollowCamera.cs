using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float acceleration;
    public float lookAhead;
    public bool followPlayer;
    public float shakeMag;
    public float shakeDur;
    public float shakeSpeed;

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

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 original = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = original.x + (Random.Range(-1f, 1f) * magnitude);
            float y = original.y + (Random.Range(-1f, 1f) * magnitude);
            transform.position = Vector3.Lerp(transform.position, new Vector3(x,y, original.z), Time.deltaTime*shakeSpeed);
            //transform.position = new Vector3(x, y, original.z);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = original;
    }

    public Transform FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player.transform;
    }
}