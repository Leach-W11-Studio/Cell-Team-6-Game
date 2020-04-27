using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float minZoom;
    public float acceleration;
    public float shakeMag;
    public float shakeDur;
    public float shakeSpeed;

    public List<Transform> targets;

    public static FollowCamera instance;

    public float targetZoom;

    private Camera _cam;

    private Camera cam
    {
        get
        {
            if (!_cam)
            {
                _cam = GetComponent<Camera>();
                return _cam;
            }
            else { return _cam; }
        }
    }

    private void Start()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        else
        {
            instance = this;
        }

        FindPlayer();
    }

    private void Update()
    {
        CleanupTargets();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Move();
        SetZoom();
    }

    public void AddTarget(Transform target)
    {
        if (!targets.Contains(target))
        {
            targets.Add(target);
        }
    }

    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
    }

    void CleanupTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.RemoveAt(i);
                i--;
            }
        }
    }

    Vector2 GetFocus(out float diagonal)
    {
        if (targets.Count == 0)
        {
            diagonal = 0;
            return new Vector2(transform.position.x, transform.position.y);
        }

        if (targets.Count == 1)
        {
            diagonal = minZoom;
            return targets[0].position;
        }

        Bounds bounds = new Bounds(targets[0].position, Vector2.zero);
        foreach (var target in targets)
        {
            if (target) { bounds.Encapsulate(target.position); } //Added null check
        }

        diagonal = (bounds.max - bounds.min).magnitude / 1.75f;
        return bounds.center;
    }

    void Move()
    {
        if (targets.Count == 0) { Debug.LogError("Could not locate target!"); FindPlayer(); return; }

        float zoom;
        Vector2 focus = GetFocus(out zoom);

        targetZoom = zoom;

        transform.position = Vector3.Lerp(transform.position, new Vector3(focus.x, focus.y, transform.position.z), Time.fixedDeltaTime * acceleration);
    }

    void SetZoom()
    {
        if (targetZoom < minZoom)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, minZoom, Time.deltaTime * acceleration);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * acceleration);
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 original = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = original.x + (Random.Range(-1f, 1f) * magnitude);
            float y = original.y + (Random.Range(-1f, 1f) * magnitude);
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, original.z), Time.fixedDeltaTime * shakeSpeed);
            //transform.position = new Vector3(x, y, original.z);
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = original;
    }

    public void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targets.Add(player.transform);
    }
}