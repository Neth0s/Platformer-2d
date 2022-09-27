using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Speed and acceleration")]
    [SerializeField] private float maxSpeed = 10f;
    [Tooltip("Time necessary to get from 0 to max speed.")]
    [SerializeField] private float timeToMax = 0.5f;

    private Manette inputActions;
    private float speed = 0;

    public float HorizontalSpeed { get { return speed; } }

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new Manette();
        inputActions.Player.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = Mathf.Lerp(speed, GetInput() * maxSpeed, Time.deltaTime / timeToMax);
        transform.position += speed * Time.deltaTime * Vector3.right;
    }


    private float GetInput()
    {
        float input = inputActions.Player.Move.ReadValue<float>();
        return input == 0 ? 0 : Mathf.Sign(input);
    }

    public void StopSpeed()
    {
        speed = 0;
    }
}
