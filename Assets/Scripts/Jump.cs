using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField, Range(0, 10)] private int maxJumps = 2;
    [SerializeField, Range(0, 50)] private float jumpImpulse = 10f;

    [Header("Falling")]
    [SerializeField] private float gravity = 10f;

    [Tooltip("Multiplier applied to gravity when falling.")]
    [SerializeField, Range(1, 10)] private float fallMultiplier = 1.5f;

    [Tooltip("Percentage of vertical speed removed if jump button is released before end of jump.")]
    [SerializeField, Range(0, 100)] private int jumpCutoff = 50;

    [SerializeField] float coyoteTime = 0.1f;
    float lastOnGroundDate = -Mathf.Infinity;

    private Manette inputActions;
    private float speed = 0;

    private int jumpsLeft;
    private bool isJumping = false;
    private bool cutoffApplied = false;

    public float VerticalSpeed { get { return speed; } }

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new Manette();
        inputActions.Player.Jump.Enable();

        jumpsLeft = maxJumps;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        speed -= (speed < 0 ? fallMultiplier : 1) * gravity * Time.deltaTime;
        transform.position += speed * Time.deltaTime * Vector3.up;
    }

    private void GetInput()
    {
        float input = inputActions.Player.Jump.ReadValue<float>();
        if (input != 0 && jumpsLeft > 0 && Time.time <= lastOnGroundDate + coyoteTime)
        {
            jumpsLeft--;
            isJumping = true;
            speed = jumpImpulse;
        }

        if (isJumping && input == 0 && speed > 0 && !cutoffApplied)
        {
            speed *= (100f - jumpCutoff) / 100f;
            cutoffApplied = true;
        }
    }

    public void TouchGround()
    {
        StopSpeed();
        lastOnGroundDate = Time.time;
        isJumping = false;
        cutoffApplied = false;
        jumpsLeft = maxJumps;
    }

    public void StopSpeed()
    {
        speed = 0;
    }
}
