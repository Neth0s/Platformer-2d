using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

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
    [SerializeField, Range(0, 100)] private int jumpCutoff = 50;
    [SerializeField] private float coyoteTime = 0.1f;


    float lastOnGroundDate = -Mathf.Infinity;
    public bool OnGround => Time.time <= lastOnGroundDate + coyoteTime;
    public float VerticalSpeed { get; private set; } = 0;

    private int jumpsLeft;
    private bool isJumping = false;
    private bool cutoffApplied = false;

    private Manette inputActions;


    void Start()
    {
        inputActions = new Manette();
        inputActions.Player.Jump.Enable();

        jumpsLeft = maxJumps;
    }

    void FixedUpdate()
    {
        GetInput();

        VerticalSpeed -= (VerticalSpeed < 0 ? fallMultiplier : 1) * gravity * Time.deltaTime;
        VerticalSpeed = Math.Clamp(VerticalSpeed, -maxDownSpeed, maxUpSpeed);

        transform.position += VerticalSpeed * Time.deltaTime * Vector3.up;
    }

    private void GetInput()
    {
        float input = inputActions.Player.Jump.ReadValue<float>();

        if (input != 0 && !isJumping && jumpsLeft > 0 && OnGround)
        {
            jumpsLeft--;
            isJumping = true;
            VerticalSpeed = VerticalSpeed > 0 ? VerticalSpeed + jumpImpulse : jumpImpulse;
        }

        if (isJumping && input == 0 && VerticalSpeed > 0 && !cutoffApplied)
        {
            VerticalSpeed *= (100f - jumpCutoff) / 100f;
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
    }

    public void StopSpeed()
    {
        VerticalSpeed = 0;
    }
}
