﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class ListClass
{
    public List<string> listSave = new List<string>();
}

public class SceneChanger : MonoBehaviour
{
    public string Next_Level;
    public Vector2 Next_Position;
    private Vector2 Position_Load;
    private Transform player_Trans;
    public HealthScript Variables;
    public ListClass Abilitysave = new ListClass(); 
    public List<Ability> currentAbilities;

    private void Start()
    {
        Variables = FindObjectOfType<PlayerController>().GetComponent<HealthScript>();
        File.Delete(Application.persistentDataPath + "/AbilitySave.txt");
        Abilitysave.listSave.Clear();
        Position_Load = Next_Position;
    }
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
        GameMaster.gameMaster.LoadLevel(Next_Level);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("PlayerHealth", Variables.currentHealth);
            SetBool("PlayerShield", Variables.sheild);

            currentAbilities = collision.GetComponentInChildren<AbilityCaster>().activeAbilities;
            foreach (Ability ability in currentAbilities)
            { Abilitysave.listSave.Add(ability.AbilityName); }
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream w = File.Open(Application.persistentDataPath + "/AbilitySave.txt", FileMode.Create))
            {
                formatter.Serialize(w, Abilitysave);
            }
            loadnext();
        }
    }
    
    public static void SetBool(string name, bool booleanValue)
    {
        PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
    }

    //This and below could also be in the health script, However I am not sure if that is what we want
    private void OnSceneLoaded(Scene thescene, LoadSceneMode amode)
    {
        Variables = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>();

        if (PlayerPrefs.GetInt("PlayerHealth") != 0)
        {
            Variables.currentHealth = PlayerPrefs.GetInt("PlayerHealth");
            Variables.sheild = GetBool("PlayerShield");
            PlayerPrefs.SetInt("PlayerHealth", Variables.maxHealth);
            SetBool("PlayerShield", false);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerHealth", Variables.maxHealth);
            SetBool("PlayerShield", false);
            Variables.currentHealth = PlayerPrefs.GetInt("PlayerHealth");
            Variables.sheild = GetBool("PlayerShield");
        }

        BinaryFormatter binary = new BinaryFormatter();
        if (new FileInfo(Application.persistentDataPath + "/AbilitySave.txt").Exists)
        {
            using (FileStream w = File.Open(Application.persistentDataPath + "/AbilitySave.txt", FileMode.Open))
            {
                Abilitysave = (ListClass)binary.Deserialize(w);
            }
        }

        AbilityCaster abil_cast = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<AbilityCaster>();
        foreach (string Abilityname in Abilitysave.listSave)
        {
            var next_abil = (GameObject)Resources.Load("Prefabs/AbilityPrefabs/" + Abilityname);
            Debug.Log("Trying to add ability" + Abilityname);
            abil_cast.AddAbility(next_abil);
        }
        if (Position_Load != new Vector2(0, 0))
        {
            player_Trans = GameObject.FindGameObjectWithTag("Player").transform;
            player_Trans.position = Position_Load;
        }
    }

    public static bool GetBool(string name)
    {
        return PlayerPrefs.GetInt(name) == 1 ? true : false;
    }
}
