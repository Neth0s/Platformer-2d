using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public float Timer { get; private set; } = 0;
    private bool levelEnded = false;

    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!levelEnded)
        {
            Timer += Time.deltaTime;
            text.text = Timer.ToString("00.00");
        }
    }

    public void EndLevel()
    {
        levelEnded = true;
    }
}
