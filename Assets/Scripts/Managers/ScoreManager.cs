using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    private static readonly float[] scores = new float[10];

    public static void UpdateScore(int level, float value)
    {
        scores[level - 1] = value;
    }

    public static float GetScore(int level)
    {
        return scores[level - 1];
    }
}
