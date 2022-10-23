using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction { Left, Right, None };

public class HorizontalMovement : MonoBehaviour
{
    [Header("Speed and acceleration")]
    [SerializeField, Min(0)] private float acceleration = 40f;
    [SerializeField, Min(0)] private float desceleration = 40f;
    [SerializeField, Min(0)] private float maxSpeed = 10f;

    [Header("Ground controls")]
    [SerializeField, Min(0)] private float turnSpeed = 5f;

    [Header("Aerial control")]
    [SerializeField, Range(0, 1)] private float airControl = 0.5f;
    [SerializeField, Range(0, 1)] private float airBrake = 0.5f;
    [SerializeField, Min(0)] private float airTurnSpeed = 5f;

    [Header("Dash")]
    [SerializeField, Min(0)] private float dashSpeed = 20f;
    [SerializeField, Min(0)] private float dashTime = 0.1f;
    [SerializeField, Min(0)] private float dashReloadTime = 1f;

    [Header("Animation")]
    [SerializeField, Range(-30, 30)] private float runAngle = -15f;
    [SerializeField] private Color dashingColor;
    [SerializeField] private Color dashEmptyColor;
    [SerializeField, Min(0)] private float trailTime = 0.25f;
    [SerializeField] private Settings settings;

    private float input = 0;
    public float Speed { get; set; } = 0;
    public bool AirBrakeApplied { get; set; } = false;

    private Direction dashDirection = Direction.Right;
    float lastDashDate = -Mathf.Infinity;

    public enum DashState { Idle, Dashing, Cooldown }
    public DashState IsDashing { get; private set; } = DashState.Idle;

    public float WallJumpEnd { get; set; } = -Mathf.Infinity;

    private Manette manette;
    private Jump jumpController;
    private SpriteRenderer sprite;
    private SpriteAnimator animator;
    private TrailRenderer trail;

    void Awake()
    {
        manette = new Manette();
        manette.Player.Move.Enable();
        manette.Player.Dash.Enable();

        jumpController = GetComponent<Jump>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<SpriteAnimator>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        manette.Player.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        manette.Player.Dash.performed -= OnDash;
    }

    void FixedUpdate()
    {
        input = manette.Player.Move.ReadValue<float>();

        Movement();
        UpdateDashState();

        transform.position += Speed * Time.deltaTime * Vector3.right;
    }

    private void Movement()
    {
        bool onGround = jumpController.OnGround;

        if (!onGround) input *= airControl;
        else animator.Rotate(runAngle * Speed / maxSpeed);

        if (IsDashing == DashState.Dashing)
        {
            Speed = dashSpeed * (dashDirection == Direction.Right ? 1 : -1);
            jumpController.StopSpeed();
            return;
        }

        if (WallJumpEnd != -Mathf.Infinity)
        {
            if (WallJumpEnd < Time.time) WallJumpEnd = -Mathf.Infinity;
            return;
        }

        if (input != 0)
        {
            float turnMultiplier = 1;
            if (Speed > 0 && input < 0 || Speed < 0 && input > 0)
            {
                if (onGround) turnMultiplier = turnSpeed;
                else turnMultiplier = airTurnSpeed;
            }

            Speed += turnMultiplier * acceleration * input * Time.deltaTime;
            Speed = Mathf.Clamp(Speed, -maxSpeed, maxSpeed);
        }
        else
        {
            float brakeMultiplier = onGround ? 1 : airBrake;

            if (Speed > 0)
            {
                Speed -= brakeMultiplier * desceleration * Time.deltaTime;
                Speed = Mathf.Max(0, Speed);
            }
            else if (Speed < 0)
            {
                Speed += brakeMultiplier * desceleration * Time.deltaTime;
                Speed = Mathf.Min(0, Speed);
            }
        }
    }

    private void OnDash(InputAction.CallbackContext obj)
    {
        if (manette.Player.Dash.ReadValue<float>() != 0 && Time.time >= lastDashDate + dashReloadTime)
        {
            IsDashing = DashState.Dashing;
            lastDashDate = Time.time;
            dashDirection = input >= 0 ? Direction.Right : Direction.Left;

            if (settings.DashEffects)
            {
                sprite.color = dashingColor;
                trail.enabled = true;
            }
        }
    }

    private void UpdateDashState()
    {
        if (IsDashing == DashState.Idle) return;

        if (IsDashing == DashState.Dashing && Time.time >= lastDashDate + dashTime)
        {
            IsDashing = DashState.Cooldown;
            if (settings.DashEffects) sprite.color = dashEmptyColor;

            Speed = Mathf.Clamp(Speed, -maxSpeed, maxSpeed);
        }

        if (Time.time >= lastDashDate + dashTime + trailTime) trail.enabled = false;

        if (IsDashing == DashState.Cooldown && Time.time >= lastDashDate + dashReloadTime)
        {
            IsDashing = DashState.Idle;
            if (settings.DashEffects) sprite.color = Color.white;
        }
    }

    public void StopSpeed()
    {
        Speed = 0;
    }

    public void EnableCommands(bool active)
    {
        if (active) manette.Player.Enable();
        else manette.Player.Disable();
    }
}
