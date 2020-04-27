using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    public HealthScript healthScript;
    public Color normalColor;
    public Color invulnColor;

    private bool m_visable;
    public bool visable
    {
        get { return m_visable; }
        set
        {
            m_visable = value;
            gameObject.SetActive(m_visable);
        }
    }

    private RectTransform healthBarVisable;
    private Image healthBarVisImage;

    // Start is called before the first frame update
    void Start()
    {
        //healthScript = FindObjectOfType<BossEnemy>().GetComponent<HealthScript>();
        visable = false;

        healthBarVisable = transform.GetChild(0).GetComponent<RectTransform>();
        healthBarVisImage = healthBarVisable.gameObject.GetComponent<Image>();
        healthBarVisable.localScale = new Vector3(GetHealthPercent(), healthBarVisable.localScale.y, healthBarVisable.localScale.z);
    }

    private float GetHealthPercent()
    {
        if (healthScript)
        {
            //Debug.Log((float)healthScript.currentHealth / (float)healthScript.maxHealth);
            return ((float)healthScript.currentHealth / (float)healthScript.maxHealth);
        }
        else { return 0f; }
    }

    // Update is called once per frame
    void Update()
    {
        healthBarVisable.localScale = new Vector3(GetHealthPercent(), healthBarVisable.localScale.y, healthBarVisable.localScale.z);
        if (!healthScript.invincible) { healthBarVisImage.color = normalColor; }
        else { healthBarVisImage.color = invulnColor; }

        if (GetHealthPercent() <= 0) { visable = false; }
    }
}
