using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float minJumpHeight = 2.5f;
    [SerializeField] private float maxJumpTime = 0.5f;
    [SerializeField] private float gravity = 9f;

    private float speed = 0;

    private int jumpsLeft;
    private bool isJumping = false;

    public bool OnGround { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        jumpsLeft = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.zero;
    }
}
