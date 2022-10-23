using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    [SerializeField] Settings settings;

    public void ResetValues()
    {
        ScoreManager.ResetScores();

        settings.DashEffects = true;
        settings.DeathEffects = true;
        settings.MovementParticles = true;

        SceneManager.LoadScene(0);
    }
}
