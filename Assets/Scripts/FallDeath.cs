using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallDeath : MonoBehaviour
{
    [SerializeField] float yThreshold = -5f;

    [SerializeField] UnityEvent onFallDeath;

    private void Update()
    {
        if(transform.position.y < yThreshold)
        {
            onFallDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
