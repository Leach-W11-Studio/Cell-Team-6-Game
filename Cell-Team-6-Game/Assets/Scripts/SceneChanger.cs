using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public string Next_Level;
    public HealthScript Variables;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void loadnext()
    {
        SceneManager.LoadScene(Next_Level);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt("PlayerHealth", Variables.currentHealth);
        SetBool("PlayerShield", Variables.sheild);
        loadnext();
    }

    public static void SetBool(string name, bool booleanValue)
    {
        PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
    }

    //This and below could also be in the health script, However I am not sure if that is what we want
    private void OnSceneLoaded(Scene thescene, LoadSceneMode amode)
    {
        Variables = GameObject.Find("Player").GetComponent<HealthScript>();
        Variables.currentHealth = PlayerPrefs.GetInt("PlayerHealth");
        Variables.sheild = GetBool("PlayerShield");
        PlayerPrefs.SetInt("PlayerHealth", Variables.maxHealth);
        SetBool("PlayerShield", false);
    }

    public static bool GetBool(string name)
    {
        return PlayerPrefs.GetInt(name) == 1 ? true : false;
    }
}
