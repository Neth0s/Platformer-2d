using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private int level;
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        float score = ScoreManager.GetScore(level);
        if (score > 0) text.text = string.Format("Best: {0:F2}s", score);
        else text.text = string.Empty;
    }
}
