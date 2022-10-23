using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class ScoreManager
{

    public static void UpdateScore(int level, float value)
    {
        if (value < PlayerPrefs.GetFloat("level"+level, Mathf.Infinity))
            PlayerPrefs.SetFloat("level" + level, value);

        PlayerPrefs.Save();
    }

    public static float GetScore(int level)
    {
        return PlayerPrefs.GetFloat("level" + level, 0);
    }

    public static void ResetScores()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
