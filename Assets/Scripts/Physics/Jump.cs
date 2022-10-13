using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class Jump : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField, Min(0)] private float maxUpSpeed = 30f;
    [SerializeField, Min(0)] private float maxDownSpeed = 50f;
    [SerializeField, Min(0)] private float fastFallSpeed = 30f;

    [Header("Jump parameters")]
    [SerializeField, Min(0)] private int maxJumps = 2;
    [SerializeField, Min(0)] private float jumpImpulse = 10f;
    [SerializeField] private GameObject burstParticles;

    [Header("Wall Jump")]
    [SerializeField, Min(0)] private float wallJumpImpulse = 8f;
    [SerializeField, Range(0, 90)] private float wallJumpAngle = 45f;
    [SerializeField, Min(0)] private float wallJumpTime = 0.25f;

    [Header("Falling")]
    [SerializeField] private float gravity = 10f;
    [SerializeField, Min(0)] private float wallSlideSpeed = 5f;
    [SerializeField, Range(1, 10)] private float fallMultiplier = 1.5f;
    [Tooltip("Percentage of vertical speed removed if jump button is released before end of jump.")]
    [SerializeField, Range(0, 1)] private float jumpCutoff = 0.5f;

    [Header("Buffers")]
    [SerializeField, Min(0)] private float jumpBuffer = 0.1f;
    [SerializeField, Min(0)] private float coyoteTime = 0.1f;

    [Header("Animation")]
    [SerializeField, Range(1, 2)] private float jumpSquash = 1.5f;
    [SerializeField, Range(1, 2)] private float landSquash = 1.5f;
    [SerializeField, Range(0, 1)] private float fastfallSquash = 0.25f;
    [SerializeField, Range(1, 2)] private float fastfallStretch = 1.5f;

    public bool OnGround => Time.time <= lastOnGroundDate + coyoteTime;
    public Direction OnWall { get; set; } = Direction.None;
    public float VerticalSpeed { get; private set; } = 0;

    private bool leftGround = false;
    private float lastOnGroundDate = -Mathf.Infinity;
    private float lastJumpTap = -Mathf.Infinity;

    private int jumpsLeft;
    private float wallJumpRadian;
    private bool isJumping = false;
    private bool isFastfall = false;
    private bool cutoffApplied = false;

    private HorizontalMovement movement;
    private ParticleSystem particles;
    private SpriteAnimator animator;

    private Manette inputActions;

    private void OnDrawGizmos()
    {
        wallJumpRadian = wallJumpAngle * Mathf.PI / 180f;

        Vector3 wallJumpTarget = transform.position;
        wallJumpTarget.x += wallJumpImpulse * Mathf.Cos(wallJumpRadian) / 10;
        wallJumpTarget.y += wallJumpImpulse * Mathf.Sin(wallJumpRadian) / 10;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, wallJumpTarget);
    }

    void Awake()
    {
        movement = GetComponent<HorizontalMovement>();
        particles = GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<SpriteAnimator>();

        inputActions = new Manette();
        inputActions.Player.Jump.Enable();
        inputActions.Player.Fastfall.Enable();

        jumpsLeft = maxJumps;
        wallJumpRadian = wallJumpAngle *Mathf.PI / 180f;
    }

    private void OnEnable()
    {
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;
    }

    void FixedUpdate()
    {
        GetInput();
        if (!OnGround && !leftGround) LeaveGround();

        VerticalSpeed -= (VerticalSpeed < 0 ? fallMultiplier : 1) * gravity * Time.deltaTime;

        float maxFallSpeed = OnWall != Direction.None ? -wallSlideSpeed : -maxDownSpeed;
        VerticalSpeed = Math.Clamp(VerticalSpeed, maxFallSpeed, maxUpSpeed);

        transform.position += VerticalSpeed * Time.deltaTime * Vector3.up;
    }

    private void GetInput()
    {
        if (isJumping && !cutoffApplied && VerticalSpeed > 0)
        {
            //Didn't release jump
            if (inputActions.Player.Jump.ReadValue<float>() != 0) return;

            VerticalSpeed *= 1 - jumpCutoff;
            cutoffApplied = true;
        }

        if (isJumping && !isFastfall && OnWall == Direction.None &&
            inputActions.Player.Fastfall.ReadValue<float>() != 0)
        {
            VerticalSpeed = -fastFallSpeed;
            isFastfall = true;
            animator.FastStretch(fastfallSquash, fastfallStretch);
        }
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (OnWall != Direction.None)
        {
            isJumping = true;
            VerticalSpeed = wallJumpImpulse * Mathf.Sin(wallJumpRadian);
            movement.Speed = wallJumpImpulse * Mathf.Cos(wallJumpRadian);

            if (OnWall == Direction.Right) movement.Speed *= -1;
            movement.WallJumpEnd = Time.time + wallJumpTime;
        }
        else JumpAction();

    }

    private void JumpAction()
    {
        if (jumpsLeft > 0)
        {
            LeaveGround();

            jumpsLeft--;
            isJumping = true;
            VerticalSpeed = jumpImpulse;

            Destroy(Instantiate(burstParticles, transform), 1);
            animator.StretchLoop(1/jumpSquash, jumpSquash);
        }
        else lastJumpTap = Time.time;
    }

    private void LeaveGround()
    {
        leftGround = true;
        particles.Stop();
        animator.ResetRotation();
    }

    public void TouchGround(float bounciness)
    {
        VerticalSpeed = -VerticalSpeed * bounciness;
        lastOnGroundDate = Time.time;

        if (!leftGround) return;

        isJumping = false;
        isFastfall = false;
        cutoffApplied = false;
        jumpsLeft = maxJumps;

        movement.AirBrakeApplied = false;

        leftGround = false;
        animator.StretchLoop(landSquash, 1 / landSquash);
        particles.Play();

        if (lastJumpTap != -Mathf.Infinity)
        {
            if (Time.time <= lastJumpTap + jumpBuffer) JumpAction();
            else lastJumpTap = -Mathf.Infinity;
        }
    }

    public void StopSpeed() => VerticalSpeed = 0;
}