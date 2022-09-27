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
        var contactFilter = new ContactFilter2D().NoFilter();
        var results = new List<Collider2D>();

        var horizontalSpeed = horizontalMovement.HorizontalSpeed;
        var verticalSpeed = jump.VerticalSpeed;

        var initialPosition = transform.position;

        coll.OverlapCollider(contactFilter, results);

        if (horizontalSpeed > 0)
        {
            var collisions = Physics2D.OverlapBoxAll(transform.position + (Vector3)(Vector2.right * horizontalSpeed * Time.fixedDeltaTime), coll.size, 0);
            var goodCollisions = new List<Collider2D>();
            foreach (var collision in collisions)
                if (collision.gameObject != gameObject)
                    goodCollisions.Add(collision);

            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(collisions[0].bounds.min.x - coll.size.x * transform.localScale.x / 2 - epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }
        else if(horizontalSpeed < 0)
        {
            var collisions = Physics2D.OverlapBoxAll(transform.position + (Vector3)(Vector2.right * horizontalSpeed * Time.fixedDeltaTime), coll.size, 0);
            var goodCollisions = new List<Collider2D>();
            foreach (var collision in collisions)
                if (collision.gameObject != gameObject)
                    goodCollisions.Add(collision);

            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(collisions[0].bounds.max.x + coll.size.x * transform.localScale.x / 2 + epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }

        if (verticalSpeed > 0)
        {
            var collisions = Physics2D.OverlapBoxAll(transform.position + (Vector3)(Vector2.up * verticalSpeed * Time.fixedDeltaTime), coll.size, 0);
            var goodCollisions = new List<Collider2D>();
            foreach (var collision in collisions)
                if(collision.gameObject != gameObject)
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
                if (collision.gameObject != gameObject)
                    goodCollisions.Add(collision);

            if (goodCollisions.Count > 0)
            {
                transform.position = new Vector3(transform.position.x, goodCollisions[0].bounds.max.y + coll.size.y * transform.localScale.y / 2 + epsilon, transform.position.z);
                jump.StopSpeed();
            }
        }
    }
}
