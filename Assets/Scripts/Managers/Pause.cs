using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button settingsButton;

    [Header("Player")]
    [SerializeField] private GameObject player;

    private Manette manette;
    private HorizontalMovement playerHorizontal;
    private Jump playerJump;

    private void Awake()
    {
        manette = new Manette();
        manette.UI.Pause.Enable();

        playerHorizontal = player.GetComponent<HorizontalMovement>();
        playerJump = player.GetComponent<Jump>();
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
            EnablePlayerCommands(true);
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
            EnablePlayerCommands(false);
        }
    }

    private void EnablePlayerCommands(bool active)
    {
        playerHorizontal.EnableCommands(active);
        playerJump.EnableCommands(active);
    }
}
