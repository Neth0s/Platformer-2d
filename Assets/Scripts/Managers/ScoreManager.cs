using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{

    public static void UpdateScore(int level, float value)
    {
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
