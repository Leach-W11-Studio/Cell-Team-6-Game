using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PlayerInventory))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    [Tooltip("The number to divide the moveSpeed by when walking")]
    public float walkModifier;
    public float explosionBufferTime;

    public bool isWalking;
    public bool canExplode = true;

    GameObject hitboxHighlight;
    PlayerInventory inventory;

    private void Start()
    {
        hitboxHighlight = transform.Find("Hitbox Highlight").gameObject;
        inventory = GetComponent<PlayerInventory>();
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

    void CreateExplosion(string cellType) {
        // check if canExplode and the amount of this particular cell is greater than 0
        if (canExplode && inventory.whiteBloodCells[cellType] > 0) {
            // decrement cell by 1
            inventory.RemoveBloodCell(cellType);
            // spawn explosion at player position
            GameObject explosion = Instantiate(Resources.Load<GameObject>("Prefabs/Blood Cell Explosion"), transform.position, Quaternion.identity, null);
            // start canExplode timer
            StartCoroutine(ExplosionBuffer());
        }
    }

    private void Update()
    {
        if (Input.GetButton("Walk")) { isWalking = true; }
        else { isWalking = false; }

        if (Input.GetButtonDown("Explosion1")) { CreateExplosion("Dual Shot"); }
        else if (Input.GetButtonDown("Explosion2")) { CreateExplosion("Spread Shot"); }
        else if (Input.GetButtonDown("Explosion3")) { CreateExplosion("Ricochet"); }
        else if (Input.GetButtonDown("Explosion4")) { CreateExplosion("Mega Shot"); }
        else if (Input.GetButtonDown("Explosion5")) { CreateExplosion("Stun"); }

        if (isWalking) { hitboxHighlight.SetActive(true); }
        else { hitboxHighlight.SetActive(false); }

        if (Input.GetButtonDown("Cancel")) { Application.Quit(); }
    }

    void FixedUpdate()
    {
        Movement();
        MousePoint();
    }

    IEnumerator ExplosionBuffer() {
        canExplode = false;
        yield return new WaitForSeconds(explosionBufferTime);
        canExplode = true;
    }
}
