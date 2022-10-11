using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Jump : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField, Min(0)] private float maxUpSpeed = 30f;
    [SerializeField, Min(0)] private float maxDownSpeed = 50f;

    [Header("Jump parameters")]
    [SerializeField, Range(0, 10)] private int maxJumps = 2;
    [SerializeField, Range(0, 50)] private float jumpImpulse = 10f;

    [Header("Falling")]
    [SerializeField] private float gravity = 10f;
    [SerializeField, Range(1, 10)] private float fallMultiplier = 1.5f;
    [Tooltip("Percentage of vertical speed removed if jump button is released before end of jump.")]
    [SerializeField, Range(0, 1)] private float jumpCutoff = 0.5f;

    [Header("Buffers")]
    [SerializeField] private float jumpBuffer = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;

    public bool OnGround => Time.time <= lastOnGroundDate + coyoteTime;
    public float VerticalSpeed { get; private set; } = 0;

    float lastOnGroundDate = -Mathf.Infinity;
    float lastJumpTap = -Mathf.Infinity;

    private int jumpsLeft;
    private bool isJumping = false;
    private bool cutoffApplied = false;

    private Manette inputActions;


    void Awake()
    {
        inputActions = new Manette();
        inputActions.Player.Jump.Enable();

        jumpsLeft = maxJumps;
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

        VerticalSpeed -= (VerticalSpeed < 0 ? fallMultiplier : 1) * gravity * Time.deltaTime;
        VerticalSpeed = Math.Clamp(VerticalSpeed, -maxDownSpeed, maxUpSpeed);

        transform.position += VerticalSpeed * Time.deltaTime * Vector3.up;
    }

    private void OnJump(InputAction.CallbackContext obj) => JumpAction();

    private void JumpAction()
    {
        if (jumpsLeft > 0)
        {
            jumpsLeft--;
            isJumping = true;
            VerticalSpeed = VerticalSpeed > 0 ? VerticalSpeed + jumpImpulse : jumpImpulse;
        }
        else lastJumpTap = Time.time;
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
    }

    public void TouchGround(float bounciness)
    {
        VerticalSpeed = -VerticalSpeed * bounciness;
        lastOnGroundDate = Time.time;
        isJumping = false;

        cutoffApplied = false;
        jumpsLeft = maxJumps;

        GetComponent<HorizontalMovement>().AirBrakeApplied = false;

        if (lastJumpTap != -Mathf.Infinity)
        {
            if (Time.time <= lastJumpTap + jumpBuffer) JumpAction();
            else lastJumpTap = -Mathf.Infinity;
        }
    }

    public void StopSpeed()
    {
        VerticalSpeed = 0;
    }
}
