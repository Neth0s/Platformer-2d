using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    BoxCollider2D coll;
    HorizontalMovement horizontalMovement;
    Jump jump;

    const float epsilon = 0.001f;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        horizontalMovement = GetComponent<HorizontalMovement>();
        jump = GetComponent<Jump>();
    }

    private void FixedUpdate()
    {
        // Detect collisions a frame ahead.
        var nextPosition = transform.position 
            + (Vector3)(horizontalMovement.Speed * Time.fixedDeltaTime * Vector2.right) 
            + (Vector3)(jump.VerticalSpeed * Time.fixedDeltaTime * Vector2.up);

        var collisions = Physics2D.OverlapBoxAll(nextPosition, coll.size, 0);

        // Evaluate the direction of each collision.
        var leftCollisions = new List<Collider2D>();
        var rightCollisions = new List<Collider2D>();
        var bottomCollisions = new List<Collider2D>();
        var upCollisions = new List<Collider2D>();

        foreach (var collision in collisions)
        {
            if (collision.gameObject != gameObject)
            {
                if (coll.bounds.max.x < collision.bounds.min.x) leftCollisions.Add(collision);
                else if (coll.bounds.min.x > collision.bounds.max.x) rightCollisions.Add(collision);
                else if (coll.bounds.max.y < collision.bounds.min.y) bottomCollisions.Add(collision);
                else if (coll.bounds.min.y > collision.bounds.max.y) upCollisions.Add(collision);
            }
        }

        // Resolve collisions based on their direction.
        // This could be improved by choosing the best collision candidate within each collision list.
        // However, we decided to prioritize other features as it worked already good enough for this project.
        if (leftCollisions.Count > 0)
        {
            transform.position = new Vector3(leftCollisions[0].bounds.min.x - coll.size.x * transform.localScale.x / 2 - epsilon, transform.position.y, transform.position.z);
            horizontalMovement.StopSpeed();
            jump.OnWall = Direction.Right;
        }
        if (rightCollisions.Count > 0)
        {
            transform.position = new Vector3(rightCollisions[0].bounds.max.x + coll.size.x * transform.localScale.x / 2 + epsilon, transform.position.y, transform.position.z);
            horizontalMovement.StopSpeed();
            jump.OnWall = Direction.Left;
        }
        if (bottomCollisions.Count > 0)
        {
            transform.position = new Vector3(transform.position.x, bottomCollisions[0].bounds.min.y - coll.size.y * transform.localScale.y / 2 - epsilon, transform.position.z);
            jump.StopSpeed();
        }
        if (upCollisions.Count > 0)
        {
            transform.position = new Vector3(transform.position.x, upCollisions[0].bounds.max.y + coll.size.y * transform.localScale.y / 2 + epsilon, transform.position.z);
            jump.TouchGround(upCollisions[0].bounciness);
        }

        if (leftCollisions.Count == 0 && rightCollisions.Count == 0) jump.OnWall = Direction.None;
    }
}
