using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    enum Direction { Left, Right };

    [Header("Speed and acceleration")]
    [SerializeField, Min(0)] private float acceleration = 40f;
    [SerializeField, Min(0)] private float desceleration = 40f;
    [SerializeField, Min(0)] private float maxSpeed = 10f;

    [Header("Ground controls")]
    [SerializeField, Min(0)] private float turnSpeed = 5f;

    [Header("Aerial control")]
    [SerializeField, Range(0, 100)] private float airControl = 50f;
    [SerializeField, Range(0, 100)] private float airBrake = 50f;
    [SerializeField, Min(0)] private float airTurnSpeed = 5f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashReloadTime = 1f;

    [SerializeField] private Color dashingColor;
    [SerializeField] private Color dashEmptyColor;

    private float input = 0;
    public float Speed { get; private set; } = 0;
    public bool AirBrakeApplied { get; set; } = false;

    private Direction playerDirection = Direction.Right;
    private Direction dashDirection = Direction.Right;

    float lastDashDate = -Mathf.Infinity;

    public enum DashState { Idle, Dashing, Cooldown }
    public DashState IsDashing { get; private set; } = DashState.Idle;

    private Manette inputActions;
    private Jump jumpController;

    void Awake()
    {
        inputActions = new Manette();
        inputActions.Player.Move.Enable();
        inputActions.Player.Dash.Enable();

        jumpController = GetComponent<Jump>();
    }

    private void OnEnable()
    {
        inputActions.Player.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        inputActions.Player.Dash.performed -= OnDash;
    }

    void FixedUpdate()
    {
        input = inputActions.Player.Move.ReadValue<float>();

        Movement();

        CheckForDashEnd();
    }

    private void Movement()
    {
        if (!jumpController.OnGround)
        {
            input *= airControl / 100f;

            if (!AirBrakeApplied && input == 0)
            {
                AirBrakeApplied = true;
                Speed *= (100f - airBrake) / 100f;
            }
        }

        if (IsDashing != DashState.Dashing)
        {
            if (input != 0)
            {
                float turnMultiplier = (
                    playerDirection == Direction.Right && input < 0 ||
                    playerDirection == Direction.Left && input > 0) 
                    ? turnSpeed : 1;

                Speed += turnMultiplier * acceleration * input * Time.deltaTime;
                Speed = Mathf.Clamp(Speed, -maxSpeed, maxSpeed);
            }
            else if (playerDirection == Direction.Right)
            {
                Speed -= desceleration * Time.deltaTime;
                Speed = Mathf.Max(0, Speed);
            }
            else
            {
                Speed += desceleration * Time.deltaTime;
                Speed = Mathf.Min(0, Speed);
            }
        }
        else
        {
            Speed = dashSpeed * (dashDirection == Direction.Right ? 1 : -1);
            jumpController.StopSpeed();
        }

        playerDirection = Speed >= 0 ? Direction.Right : Direction.Left;
        transform.position += Speed * Time.deltaTime * Vector3.right;
        Debug.Log(Speed);
    }

    private void OnDash(InputAction.CallbackContext obj)
    {
        if (inputActions.Player.Dash.ReadValue<float>() != 0 && Time.time >= lastDashDate + dashReloadTime)
        {
            IsDashing = DashState.Dashing;
            lastDashDate = Time.time;
            dashDirection = input >= 0 ? Direction.Right : Direction.Left;

            GetComponent<SpriteRenderer>().color = dashingColor;
        }
    }

    private void CheckForDashEnd()
    {
        if (IsDashing == DashState.Dashing && Time.time >= lastDashDate + dashTime)
        {
            IsDashing = DashState.Cooldown;
            GetComponent<SpriteRenderer>().color = dashEmptyColor;
        }

        if (IsDashing == DashState.Cooldown && Time.time >= lastDashDate + dashReloadTime)
        {
            IsDashing = DashState.Idle;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void StopSpeed()
    {
        Speed = 0;
    }
}
