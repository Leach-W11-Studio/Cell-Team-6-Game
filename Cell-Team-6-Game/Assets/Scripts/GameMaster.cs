using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gameMaster;
    public GameObject pauseMenuPrefab;
    public GameObject defeatScreenPrefab;
    public bool paused;
    public bool defeated;

    Canvas canvas;

    public GameObject pauseMenu;

    private void Awake()
    {
        if (gameMaster)
        {
            Destroy(gameMaster.gameObject);
            gameMaster = this;
        }
        else {
            gameMaster = this;
        }
    }

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Console")) { 
            Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
        }
    }

    public void RestartLevel() {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(sceneIndex);
    }

    public void LoadLevel(int sceneIndex) {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() {
        Application.Quit();
    }

    public void PauseGame() {
        paused = true;
        Time.timeScale = 0;
        if (pauseMenu) { Destroy(pauseMenu); }
        pauseMenu = Instantiate(pauseMenuPrefab, canvas.transform);
    }

    public void UnPauseGame() {
        paused = false;
        Time.timeScale = 1;
        if (pauseMenu) { pauseMenu.SetActive(false); }
    }

    public void LoseGame() {
        paused = true;
        defeated = true;
        Instantiate(defeatScreenPrefab, canvas.transform);
    }
}
