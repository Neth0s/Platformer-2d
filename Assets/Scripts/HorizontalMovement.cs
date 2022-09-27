using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;

    private Manette inputActions;
    private Vector2 movement = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new Manette();
        inputActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        movement = inputActions.Player.Move.ReadValue<Vector2>();
        transform.position += (Vector3) movement.normalized * maxSpeed * Time.deltaTime;
    }
}
