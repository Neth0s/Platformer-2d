using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    BoxCollider2D coll;
    HorizontalMovement horizontalMovement;

    float epsilon = 0.01f;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        horizontalMovement = GetComponent<HorizontalMovement>();
    }

    private void Update()
    {
        var contactFilter = new ContactFilter2D().NoFilter();
        var results = new List<Collider2D>();

        var horizontalSpeed = horizontalMovement.HorizontalSpeed;
        var verticalSpeed = 0f;

        if (horizontalSpeed > 0)
        {
            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(results[0].bounds.min.x - coll.size.x * transform.localScale.x / 2 - epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }
        else if(horizontalSpeed < 0)
        {
            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(results[0].bounds.max.x + coll.size.x * transform.localScale.x / 2 + epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }

        if (verticalSpeed > 0)
        {
            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(results[0].bounds.min.y - coll.size.y * transform.localScale.y / 2 - epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }
        else if(verticalSpeed < 0)
        {
            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(results[0].bounds.max.y + coll.size.y * transform.localScale.y / 2 + epsilon, transform.position.y, transform.position.z);
                horizontalMovement.StopSpeed();
            }
        }
    }
}
