using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FallDeath : MonoBehaviour
{
    [SerializeField] private float yThreshold = -8f;
    [SerializeField] private float restartDelay = 1f;

    [SerializeField] private GameObject deathParticles;

    private bool dead = false;

    private void Update()
    {
        if (transform.position.y < yThreshold && !dead)
        {
            dead = true;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<HorizontalMovement>().enabled = false;
            GetComponent<Jump>().enabled = false;
            StartCoroutine(RestartScene());
        }
    }

    private IEnumerator RestartScene()
    {
        GameObject particles = Instantiate(deathParticles);
        particles.transform.position = transform.position;

        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
