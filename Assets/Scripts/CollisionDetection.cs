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
        var nextPosition = transform.position + (Vector3)(Vector2.right * horizontalMovement.Speed * Time.fixedDeltaTime) + (Vector3)(Vector2.up * jump.VerticalSpeed * Time.fixedDeltaTime);
        var collisions = Physics2D.OverlapBoxAll(nextPosition, coll.size, 0);

        // Evaluate the direction of each collision.
        var leftToRightCollisions = new List<Collider2D>();
        var rightToLeftCollisions = new List<Collider2D>();
        var bottomToUpCollisions = new List<Collider2D>();
        var upToBottomCollisions = new List<Collider2D>();
        foreach (var collision in collisions)
        {
            if (collision.gameObject != gameObject && coll.bounds.max.x < collision.bounds.min.x)
                leftToRightCollisions.Add(collision);
            else if (collision.gameObject != gameObject && coll.bounds.min.x > collision.bounds.max.x)
                rightToLeftCollisions.Add(collision);
            else if (collision.gameObject != gameObject && coll.bounds.max.y < collision.bounds.min.y)
                bottomToUpCollisions.Add(collision);
            else if (collision.gameObject != gameObject && coll.bounds.min.y > collision.bounds.max.y)
                upToBottomCollisions.Add(collision);
        }

        // Resolve collisions based on their direction.
        if (leftToRightCollisions.Count > 0)
        {
            transform.position = new Vector3(leftToRightCollisions[0].bounds.min.x - coll.size.x * transform.localScale.x / 2 - epsilon, transform.position.y, transform.position.z);
            horizontalMovement.StopSpeed();
        }
        if (rightToLeftCollisions.Count > 0)
        {
            transform.position = new Vector3(rightToLeftCollisions[0].bounds.max.x + coll.size.x * transform.localScale.x / 2 + epsilon, transform.position.y, transform.position.z);
            horizontalMovement.StopSpeed();
        }
        if (bottomToUpCollisions.Count > 0)
        {
            transform.position = new Vector3(transform.position.x, bottomToUpCollisions[0].bounds.min.y - coll.size.y * transform.localScale.y / 2 - epsilon, transform.position.z);
            jump.StopSpeed();
        }
        if (upToBottomCollisions.Count > 0)
        {
            transform.position = new Vector3(transform.position.x, upToBottomCollisions[0].bounds.max.y + coll.size.y * transform.localScale.y / 2 + epsilon, transform.position.z);
            jump.TouchGround();
        }
    }
}
