using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private bool isPaused;

    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject playerScripts;

    public SFXController sfx;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    public void ActivateMenu()
    {
        pauseMenu.SetActive(true);
        AudioListener.pause = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        hud.SetActive(false);
        playerScripts.GetComponentInChildren<FirstPersonLook>().enabled = false;
        playerScripts.GetComponent<PlayerControl>().myAgent.enabled = false;
        playerScripts.GetComponentInChildren<Raycast>().enabled = false;
    }

    public void DeactivateMenu()
    {
        pauseMenu.SetActive(false);
        isPaused = false;

        AudioListener.pause = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        
        hud.SetActive(true);
        playerScripts.GetComponentInChildren<FirstPersonLook>().enabled = true;
        playerScripts.GetComponent<PlayerControl>().myAgent.enabled = true;
        playerScripts.GetComponentInChildren<Raycast>().enabled = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
