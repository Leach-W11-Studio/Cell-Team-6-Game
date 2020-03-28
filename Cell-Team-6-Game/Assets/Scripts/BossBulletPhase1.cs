using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletPhase1 : SimpleBullet
{

    public float amplitude;
    public float period;
    public float distance = 0;
    public float maxDistance = 10;
    public float minVariance = 0.2f;
    
    private float variedAplitude;
    private float variedPeriod;
    private Vector2 up = Vector3.up;
    private Vector2 startPos;

    override protected void Shoot()
    {
        rb.velocity = Vector2.zero;
        distance = 0;

        variedAplitude = Random.Range(amplitude * minVariance, amplitude);
        variedPeriod = Random.Range(period * minVariance, period);
        startPos = transform.position;
        up = transform.up;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (enabled) {
            if (distance > maxDistance) {
                gameObject.SetActive(false);
            }

            transform.position = GetPoint();

            distance += (Time.deltaTime * power);
        }
    }

    private float GetY() {
        if (Application.isPlaying)
        {
            return variedAplitude * Mathf.Sin(variedPeriod * distance);
        }
        else {
            return amplitude * Mathf.Sin(period * distance);
        }
    }

    private Vector2 GetAxis() {
        return startPos + (up * distance);
    }

    private Vector2 GetAxis(float distance)
    {
        return startPos + (up * distance);
    }

    private Vector2 GetPoint() {
        Vector2 axis = GetAxis();
        Vector2 perp = Vector2.Perpendicular(up);
        Vector2 point = GetY() * perp;
        return axis + point;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 perp = Vector2.Perpendicular(up);
        if (!Application.isPlaying)
        {
            startPos = transform.position;
            up = transform.up;
        }
        Gizmos.color = Color.red;

        Vector2 axisPoint = GetAxis();

        Gizmos.DrawWireSphere(axisPoint, 1f);
        Debug.DrawLine(startPos, axisPoint, Color.red);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetPoint(), 1f);

        Vector2 endPoint = GetAxis(maxDistance);
        Debug.DrawLine(endPoint + (perp * -5), endPoint + (perp * 5), Color.red);
    }
}
