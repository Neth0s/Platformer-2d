using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    [SerializeField] Settings settings;

    public void ResetScores(GameObject levelsPanel)
    {
        ScoreManager.ResetScores();
        levelsPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }

    public void ResetSettings()
    {
        settings.Particles = true;
        settings.Animations = true;
        settings.Vibrations = true;
    }
}
