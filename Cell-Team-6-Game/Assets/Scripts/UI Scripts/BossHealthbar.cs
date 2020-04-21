using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    public HealthScript healthScript;

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

    // Start is called before the first frame update
    void Start()
    {
        healthBarVisable = transform.GetChild(0).GetComponent<RectTransform>();
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
        if (GetHealthPercent() <= 0) { visable = false; }
    }
}
