using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    void Movement()
    {
        //Makes object move in an absolute fashion in all 4 directions
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed/10;
        float vertical = Input.GetAxis("Vertical") * moveSpeed/10;
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

    void FixedUpdate()
    {
        Movement();
        MousePoint();
    }
}
