using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void PauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }
}
