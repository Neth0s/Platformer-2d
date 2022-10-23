using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FallDeath : MonoBehaviour
{
    [SerializeField] private float restartDelay = 1f;

    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Settings settings;

    private bool dead = false;
    private float yThreshold;

    private void Start()
    {
        yThreshold = 1 - GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize;
    }

    private void Update()
    {
        if (transform.position.y < yThreshold && !dead) Die();
    }

    public void Die()
    {
        Clock clock = FindObjectOfType<Clock>();
        if (clock != null) clock.EndLevel();

        DisablePlayer();
        StartCoroutine(RestartScene());
    }

    public void DisablePlayer()
    {
        dead = true;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<HorizontalMovement>().enabled = false;
        GetComponent<Jump>().enabled = false;
    }

    private IEnumerator RestartScene()
    {
        if(settings.DeathEffects)
            Instantiate(deathParticles, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
