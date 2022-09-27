using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float minJumpHeight = 2.5f;
    [Tooltip("Max time jump button")]
    [SerializeField] private float maxJumpTime = 0.5f;

    [Header("Falling")]
    [SerializeField] private float gravity = 10f;

    private float speed = 0;

    private int jumpsLeft;
    private bool isJumping = false;

    public bool OnGround { get; set; } = false;
    public float VerticalSpeed { get { return speed; } }

    // Start is called before the first frame update
    void Start()
    {
        jumpsLeft = maxJumps;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!OnGround) speed -= gravity * Time.deltaTime;
        else speed = 0;

        transform.position += speed * Time.deltaTime * Vector3.up;
    }

    public void StopSpeed()
    {
        speed = 0;
    }
}
