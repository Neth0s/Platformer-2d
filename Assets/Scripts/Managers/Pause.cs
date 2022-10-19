using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject settingsMenu;

    Manette manette;

    private void Awake()
    {
        manette = new Manette();
        manette.UI.Pause.Enable();
    }

    private void OnEnable()
    {
        manette.UI.Pause.canceled += PauseMenu;
    }

    private void OnDisable()
    {
        manette.UI.Pause.canceled -= PauseMenu;
    }

    public void PauseMenu(InputAction.CallbackContext obj)
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
        else if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            settingsButton.Select();
        }
        else
        {
            pauseMenu.SetActive(true);
            settingsButton.Select();
        }
    }
}
