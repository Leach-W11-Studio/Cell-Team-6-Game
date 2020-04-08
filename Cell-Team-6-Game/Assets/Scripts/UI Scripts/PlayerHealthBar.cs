using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public GameObject heartPrefab;
    public float heartSize;
    public bool shield;

    RectTransform heartsContainer;
    HealthScript _playerHealth;
    HealthScript playerHealth {
        get {
            if (!_playerHealth) {
                Debug.Log("Player health is null.");
                _playerHealth = FindObjectOfType<PlayerController>().GetComponent<HealthScript>();
            }
            return _playerHealth;
        }
        set { _playerHealth = value; }
    }
    Image shieldIcon;

    private float lastWidth;
    private float lastSize;
    private int lastMaxHealth;
    private List<GameObject> hearts;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Awake");
        maxHealth = playerHealth.maxHealth;
        currentHealth = playerHealth.currentHealth;
        shield = playerHealth.sheild;
        shieldIcon = transform.Find("Shield").GetComponent<Image>();

        hearts = new List<GameObject>();
        heartsContainer = transform.Find("Hearts").GetComponent<RectTransform>();
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsContainer);
            RectTransform rt = heart.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heartSize);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, heartSize);
            hearts.Add(heart);
        }
        Arrange();
        lastMaxHealth = playerHealth.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerHealth.currentHealth;
        shield = playerHealth.sheild;
        if (lastMaxHealth != playerHealth.maxHealth) {
            int difference = Math.Abs(playerHealth.maxHealth - lastMaxHealth);
            if (playerHealth.maxHealth < lastMaxHealth)
            {
                for (int i = 0; i < difference; i++) {
                    Destroy(hearts[hearts.Count - 1]);
                    hearts.RemoveAt(hearts.Count - 1);
                }
            }
            else {
                for (int i = 0; i < difference; i++) {
                    GameObject heart = Instantiate<GameObject>(heartPrefab, heartsContainer);
                    RectTransform rt = heart.GetComponent<RectTransform>();
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heartSize);
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, heartSize);
                    hearts.Add(heart);
                }
            }
            Arrange();
        }

        if (lastWidth != heartsContainer.rect.width || lastSize != heartSize) {
            Arrange();
        }

        Render();

        lastWidth = heartsContainer.rect.width;
        lastSize = heartSize;
        lastMaxHealth = playerHealth.maxHealth;
    }

    private void Render()
    {
        if (shield)
        {
            shieldIcon.enabled = true;
        }
        else {
            shieldIcon.enabled = false;
        }

        for (int i = 0; i < hearts.Count; i++) {
            if (i < currentHealth)
            {
                hearts[i].GetComponent<Image>().sprite = fullHeart;
            }
            else {
                hearts[i].GetComponent<Image>().sprite = emptyHeart;
            }
        }
    }

    void Arrange() {
        float y = 0;
        float x = 0;
        foreach (GameObject heart in hearts) {
            RectTransform rt = heart.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heartSize);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, heartSize);
            if (heartsContainer.rect.width - x < heartSize) {
                y -= heartSize;
                x = 0;
            }

            rt.anchoredPosition = new Vector2(x, y);
            x += heartSize;
        }
    }
}
