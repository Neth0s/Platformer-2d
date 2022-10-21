using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    new BoxCollider2D collider;
    HorizontalMovement horizontalMovement;
    Jump jump;

    const float epsilon = 0.001f;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        horizontalMovement = GetComponent<HorizontalMovement>();
        jump = GetComponent<Jump>();
    }

    private void FixedUpdate()
    {
        // Detect collisions a frame ahead.
        var nextPosition = transform.position 
            + (Vector3)(horizontalMovement.Speed * Time.fixedDeltaTime * Vector2.right) 
            + (Vector3)(jump.VerticalSpeed * Time.fixedDeltaTime * Vector2.up);

        var collisions = Physics2D.OverlapBoxAll(nextPosition, collider.size, 0, LayerMask.GetMask("Default"));

        // Evaluate the direction of each collision.
        var rightCollisions = new List<Collider2D>();
        var leftCollisions = new List<Collider2D>();
        var topCollisions = new List<Collider2D>();
        var downCollisions = new List<Collider2D>();

        foreach (var collision in collisions)
        {
            if (collision.gameObject != gameObject)
            {
                if (collider.bounds.max.x < collision.bounds.min.x) rightCollisions.Add(collision);
                else if (collider.bounds.min.x > collision.bounds.max.x) leftCollisions.Add(collision);
                else if (collider.bounds.max.y < collision.bounds.min.y) topCollisions.Add(collision);
                else if (collider.bounds.min.y > collision.bounds.max.y) downCollisions.Add(collision);
            }
        }

        // Resolve collisions based on their direction.
        // This could be improved by choosing the best collision candidate within each collision list.
        // However, we decided to prioritize other features as it worked already good enough for this project.
        if (rightCollisions.Count > 0)
        {
            transform.position = new Vector3(rightCollisions[0].bounds.min.x - collider.size.x * transform.localScale.x / 2 - epsilon, transform.position.y, transform.position.z);
            horizontalMovement.StopSpeed();
            jump.TouchWall(Direction.Right);
        }
        if (leftCollisions.Count > 0)
        {
            transform.position = new Vector3(leftCollisions[0].bounds.max.x + collider.size.x * transform.localScale.x / 2 + epsilon, transform.position.y, transform.position.z);
            horizontalMovement.StopSpeed();
            jump.TouchWall(Direction.Left);
        }
        if (topCollisions.Count > 0)
        {
            transform.position = new Vector3(transform.position.x, topCollisions[0].bounds.min.y - collider.size.y * transform.localScale.y / 2 - epsilon, transform.position.z);
            jump.StopSpeed();
        }
        if (downCollisions.Count > 0)
        {
            transform.position = new Vector3(transform.position.x, downCollisions[0].bounds.max.y + collider.size.y * transform.localScale.y / 2 + epsilon, transform.position.z);
            jump.TouchGround(downCollisions[0].bounciness);
        }
    }
}
