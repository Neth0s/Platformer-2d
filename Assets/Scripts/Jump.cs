using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpImpulse = 2.5f;

    [Header("Falling")]
    [SerializeField] private float gravity = 10f;

    private Manette inputActions;
    private float speed = 0;

    private int jumpsLeft;
    private bool isJumping = false;

    public bool OnGround { get; private set; } = false;
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
        speed -= gravity * Time.deltaTime;
        transform.position += speed * Time.deltaTime * Vector3.up;
    }

    private void GetInput()
    {
        float input = inputActions.Player.Jump.ReadValue<float>();
        if (input != 0)
        {
            if (isJumping)
            {
                //TODO
            }
            else if (jumpsLeft > 0)
            {
                jumpsLeft--;
                OnGround = false;
                isJumping = true;
                speed = jumpImpulse;
            }
        }
    }

    public void TouchGround()
    {
        StopSpeed();
        OnGround = true;
        isJumping = false;
        jumpsLeft = maxJumps;
    }

    public void StopSpeed()
    {
        speed = 0;
    }
}
