using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Speed and acceleration")]
    [SerializeField] float maxSpeed = 10f;
    [Tooltip("Time necessary to get from 0 to max speed.")]
    [SerializeField] float timeToReachMaxSpeed = 0.5f;

    private float speed = 0;
    public float Speed { get { return speed; } }

    private Manette inputActions;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashTime = .3f;
    [SerializeField] float dashReloadTime = 1f;
    float lastDashDate = -Mathf.Infinity;
    bool isDashing = false;

    enum Direction { Left, Right };
    Direction currentDirection = Direction.Right;
    Direction lastDashDirection = Direction.Right;

    // TODO: Do the other way around and check in Jump if player isDashing rather than stop jumping here.
    Jump jump;

    void Awake()
    {
        jump = GetComponent<Jump>();

        inputActions = new Manette();
        inputActions.Player.Move.Enable();
        inputActions.Player.Dash.Enable();
    }

    private void Update()
    {
        SetCurrentDirection();

        CheckForDashStart();
        CheckForDashEnd();
    }

    void FixedUpdate()
    {
        if(!isDashing)
        {
            speed = Mathf.Lerp(speed, GetInput() * maxSpeed, Time.deltaTime / timeToReachMaxSpeed);
        }
        else
        {
            jump.StopSpeed();
            speed = dashSpeed * (lastDashDirection == Direction.Left ? -1 : 1);
        }

        transform.position += speed * Time.deltaTime * Vector3.right;
    }

    private void CheckForDashEnd()
    {
        if (isDashing && Time.time >= lastDashDate + dashTime)
        {
            isDashing = false;
        }
    }

    private void CheckForDashStart()
    {
        if (inputActions.Player.Dash.ReadValue<float>() != 0 && Time.time >= lastDashDate + dashReloadTime)
        {
            isDashing = true;
            lastDashDirection = currentDirection;
            lastDashDate = Time.time;
        }
    }

    private void SetCurrentDirection()
    {
        if (GetInput() > 0)
        {
            currentDirection = Direction.Right;
        }
        else if (GetInput() < 0)
        {
            currentDirection = Direction.Left;
        }
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
