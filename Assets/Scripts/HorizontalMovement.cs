using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Speed and acceleration")]
    [SerializeField] private float maxSpeed = 10f;
    [Tooltip("Time necessary to get from 0 to max speed.")]
    [SerializeField] private float timeToReachMaxSpeed = 0.5f;

    [Header("Aerial control")]
    [SerializeField, Range(0, 100)] private float airControl = 50f;
    [SerializeField, Range(0, 100)] private float airBrake = 50f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashReloadTime = 1f;

    [SerializeField] private Color dashingColor;
    [SerializeField] private Color dashEmptyColor;

    private float input = 0;
    private float speed = 0;
    public float Speed { get { return speed; } }

    private bool airBrakeApplied = false;

    enum Direction { Left, Right };
    Direction dashDirection = Direction.Right;

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

            if (!airBrakeApplied && input == 0)
            {
                airBrakeApplied = true;
                speed *= (100f - airBrake) / 100f;
            }
        }

        if (IsDashing != DashState.Dashing)
        {
            speed = Mathf.Lerp(speed, input * maxSpeed, Time.deltaTime / timeToReachMaxSpeed);
        }
        else
        {
            speed = dashSpeed * (dashDirection == Direction.Right ? 1 : -1);
            jumpController.StopSpeed();
        }

        transform.position += speed * Time.deltaTime * Vector3.right;
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
        speed = 0;
    }
}
