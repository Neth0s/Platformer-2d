using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    BoxCollider2D coll;
    HorizontalMovement horizontalMovement;
    Jump jump;

    float epsilon = 0.01f;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        horizontalMovement = GetComponent<HorizontalMovement>();
        jump = GetComponent<Jump>();
    }

    private void FixedUpdate()
    {
        var horizontalSpeed = horizontalMovement.HorizontalSpeed;
        var verticalSpeed = jump.VerticalSpeed;

        if (horizontalSpeed > 0)
        {
            var collisions = Physics2D.OverlapBoxAll(transform.position + (Vector3)(Vector2.right * horizontalSpeed * Time.fixedDeltaTime), coll.size, 0);
            var goodCollisions = new List<Collider2D>();
            foreach (var collision in collisions)
                if (collision.gameObject != gameObject && coll.bounds.max.x < collision.bounds.min.x)
                    goodCollisions.Add(collision);

            if (goodCollisions.Count > 0)
            {
                transform.position = new Vector3(goodCollisions[0].bounds.min.x - coll.size.x * transform.localScale.x / 2 - epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }
        else if(horizontalSpeed < 0)
        {
            var collisions = Physics2D.OverlapBoxAll(transform.position + (Vector3)(Vector2.right * horizontalSpeed * Time.fixedDeltaTime), coll.size, 0);
            var goodCollisions = new List<Collider2D>();
            foreach (var collision in collisions)
                if (collision.gameObject != gameObject && coll.bounds.min.x > collision.bounds.max.x)
                    goodCollisions.Add(collision);

            if (goodCollisions.Count > 0)
            {
                transform.position = new Vector3(goodCollisions[0].bounds.max.x + coll.size.x * transform.localScale.x / 2 + epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }

        if (verticalSpeed > 0)
        {
            var collisions = Physics2D.OverlapBoxAll(transform.position + (Vector3)(Vector2.up * verticalSpeed * Time.fixedDeltaTime), coll.size, 0);
            var goodCollisions = new List<Collider2D>();
            foreach (var collision in collisions)
                if(collision.gameObject != gameObject && coll.bounds.max.y < collision.bounds.min.y)
                    goodCollisions.Add(collision);

            if (goodCollisions.Count > 0)
            {
                transform.position = new Vector3(transform.position.x, goodCollisions[0].bounds.min.y - coll.size.y * transform.localScale.y / 2 - epsilon, transform.position.z);
                jump.StopSpeed();
            }
        }
        else if(verticalSpeed < 0)
        {
            var collisions = Physics2D.OverlapBoxAll(transform.position + (Vector3)(Vector2.up * verticalSpeed * Time.fixedDeltaTime), coll.size, 0);
            var goodCollisions = new List<Collider2D>();
            foreach (var collision in collisions)
                if (collision.gameObject != gameObject && coll.bounds.min.y > collision.bounds.max.y)
                    goodCollisions.Add(collision);

            if (goodCollisions.Count > 0)
            {
                transform.position = new Vector3(transform.position.x, goodCollisions[0].bounds.max.y + coll.size.y * transform.localScale.y / 2 + epsilon, transform.position.z);
                jump.StopSpeed();
            }
        }
    }
}
