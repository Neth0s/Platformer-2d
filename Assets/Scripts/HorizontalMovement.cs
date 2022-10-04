using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    [Header("Speed and acceleration")]
    [SerializeField] private float maxSpeed = 10f;
    [Tooltip("Time necessary to get from 0 to max speed.")]
    [SerializeField] private float timeToReachMaxSpeed = 0.5f;

    [Header("Aerial control")]
    [SerializeField, Range(0, 100)] private float airControl = 50f;
    [SerializeField, Range(0, 100)] private float airBrake = 50f;
    [SerializeField, Range(0, 100)] private float airAcceleration = 50f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashReloadTime = 1f;

    [SerializeField] private Color dashingColor;
    [SerializeField] private Color dashEmptyColor;


    private float speed = 0;
    public float Speed { get { return speed; } }

    enum Direction { Left, Right };
    Direction dashDirection = Direction.Right;

    float lastDashDate = -Mathf.Infinity;

    public enum DashState { Idle, Dashing, Cooldown }
    public DashState IsDashing { get; private set; } = DashState.Idle;

    private Manette inputActions;

    void Awake()
    {
        inputActions = new Manette();
        inputActions.Player.Move.Enable();
        inputActions.Player.Dash.Enable();
    }

    private void Update()
    {
        CheckForDashStart();
        CheckForDashEnd();
    }

    void FixedUpdate()
    {
        if(IsDashing != DashState.Dashing)
        {
            speed = Mathf.Lerp(speed, GetInput() * maxSpeed, Time.deltaTime / timeToReachMaxSpeed);
        }
        else speed = dashSpeed * (dashDirection == Direction.Right ? 1 : -1);

        transform.position += speed * Time.deltaTime * Vector3.right;
    }

    private void CheckForDashStart()
    {
        if (inputActions.Player.Dash.ReadValue<float>() != 0 && Time.time >= lastDashDate + dashReloadTime)
        {
            IsDashing = DashState.Dashing;
            lastDashDate = Time.time;
            dashDirection = GetInput() >= 0 ? Direction.Right : Direction.Left;

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
