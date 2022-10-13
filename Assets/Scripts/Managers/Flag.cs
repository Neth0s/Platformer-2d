using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] private float endDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Clock clock = FindObjectOfType(typeof(Clock)).GetComponent<Clock>();
            clock.EndLevel();

            ScoreManager.UpdateScore(SceneManager.GetActiveScene().buildIndex, clock.LevelClock);
            collision.gameObject.SetActive(false);
            StartCoroutine(EndLevel());
        }
    }

    private IEnumerator EndLevel()
    {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(endDelay);
        SceneManager.LoadScene(0);
    }
}
