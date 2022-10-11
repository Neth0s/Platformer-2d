using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FallDeath : MonoBehaviour
{
    [SerializeField] private float yThreshold = -8f;
    [SerializeField] private float restartDelay = 1f;

    [SerializeField] UnityEvent onFallDeath;

    private void Update()
    {
        if (transform.position.y < yThreshold) StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
