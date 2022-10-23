using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] private float endDelay;

    [Header("Level")]
    [SerializeField] private int currentLevel;
    [SerializeField] private string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Clock clock = FindObjectOfType<Clock>();

            if (clock != null)
            {
                clock.EndLevel();
                ScoreManager.UpdateScore(currentLevel, clock.Timer);
            }

            collision.gameObject.SetActive(false);
            StartCoroutine(EndLevel());
        }
    }

    private IEnumerator EndLevel()
    {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(endDelay);
        SceneManager.LoadScene(nextScene);
    }
}
