using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    [Tooltip("The number to divide the moveSpeed by when walking")]
    public float walkModifier;

    public bool isWalking;
    GameObject hitboxHighlight;

    private void Start()
    {
        hitboxHighlight = transform.Find("Hitbox Highlight").gameObject;
    }

    void Movement()
    {
        float targetSpeed = isWalking ? moveSpeed / walkModifier : moveSpeed;
        //Makes object move in an absolute fashion in all 4 directions
        float horizontal = Input.GetAxis("Horizontal") * targetSpeed/10;
        float vertical = Input.GetAxis("Vertical") * targetSpeed/10;
        transform.position = new Vector2(transform.position.x + horizontal, transform.position.y + vertical);
    }

    void MousePoint()
    {
        //Makes player rotate towards the mouse
        Vector2 mousePos = Input.mousePosition;
        Vector3 objPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objPos.x;
        mousePos.y = mousePos.y - objPos.y;
        float newRot = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        newRot -= 90.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, newRot));
    }

    private void Update()
    {
        if (Input.GetButton("Walk")) { isWalking = true; }
        else { isWalking = false; }

        if (isWalking) { hitboxHighlight.SetActive(true); }
        else { hitboxHighlight.SetActive(false); }
    }

    void FixedUpdate()
    {
        Movement();
        MousePoint();
    }
}
