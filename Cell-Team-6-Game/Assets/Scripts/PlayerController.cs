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
    public float invincebilityTime;

    //Added - Ben Shackman
    public PlayerGunScript gun;
    
    public bool isWalking;
    public bool canExplode = true;

    GameObject hitboxHighlight;
    PlayerInventory inventory;
    HealthScript playerHealth;
    GameObject sheild;
    SpriteRenderer playerSprite;
    private Animator PlayerAnim;

    private void Start()
    {
        gun = transform.GetComponentInChildren<PlayerGunScript>();
        hitboxHighlight = transform.Find("Hitbox Highlight").gameObject;
        inventory = GetComponent<PlayerInventory>();
        sheild = transform.Find("sheild").gameObject;
        playerHealth = GetComponent<HealthScript>();
        playerSprite = transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();
        PlayerAnim = gameObject.GetComponent<Animator>();
        playerHealth.onTakeDamage.AddListener(() => {
            StartCoroutine(Invincible());
        });
    }

    void Movement()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            PlayerAnim.SetBool("isWalking", true);
        }
        else
        {
            PlayerAnim.SetBool("isWalking", false);
        }

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

    void Firing()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            PlayerAnim.SetBool("isShooting", true);
            gun.Shoot();
        }
        else
            PlayerAnim.SetBool("isShooting", false);
    }
    
    void CreateExplosion(PlayerInventory.BulletType cellType) {
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

        //Commenting out explosions as we are apperently not doing these anymore - Ben
        /* if (Input.GetButtonDown("Explosion1")) { CreateExplosion(PlayerInventory.BulletType.DualShot); }
        else if (Input.GetButtonDown("Explosion2")) { CreateExplosion(PlayerInventory.BulletType.SpreadShot); }
        else if (Input.GetButtonDown("Explosion3")) { CreateExplosion(PlayerInventory.BulletType.RicochetShot); }
        else if (Input.GetButtonDown("Explosion4")) { CreateExplosion(PlayerInventory.BulletType.MegaShot); }
        else if (Input.GetButtonDown("Explosion5")) { CreateExplosion(PlayerInventory.BulletType.StunShot); } */

        if (isWalking) { hitboxHighlight.SetActive(true); }
        else { hitboxHighlight.SetActive(false); }

        if (Input.GetButtonDown("Cancel")) {
            if (GameMaster.gameMaster.paused && !GameMaster.gameMaster.defeated) {
                GameMaster.gameMaster.UnPauseGame();
            }
            else if (!GameMaster.gameMaster.paused && !GameMaster.gameMaster.defeated) {
                GameMaster.gameMaster.PauseGame();
            }
        }

        if (playerHealth.sheild)
        {
            sheild.SetActive(true);
        }
        else {
            sheild.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Movement();
        MousePoint();
        Firing();
    }
    
    IEnumerator ExplosionBuffer() {
        canExplode = false;
        yield return new WaitForSeconds(explosionBufferTime);
        canExplode = true;
    }

    IEnumerator Invincible() {
        playerHealth.invincible = true;
        float elapsed = 0f;
        while (elapsed < invincebilityTime) {
            if (playerSprite.enabled)
            {
                playerSprite.enabled = false;
            }
            else {
                playerSprite.enabled = true;
            }
            yield return new WaitForSeconds(0.05f);
            elapsed += 0.05f;
        }
        yield return new WaitForEndOfFrame();
        playerSprite.enabled = true;
        playerHealth.invincible = false;
    }
}