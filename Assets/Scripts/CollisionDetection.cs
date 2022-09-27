using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    BoxCollider2D coll;
    HorizontalMovement horizontalMovement;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        horizontalMovement = GetComponent<HorizontalMovement>();
    }

    private void Update()
    {
        var contactFilter = new ContactFilter2D().NoFilter();
        var results = new List<Collider2D>();

<<<<<<< HEAD
        var horizontalSpeed = horizontalMovement.HorizontalSpeed;
        var initialPosition = transform.position;

        if(horizontalSpeed > 0)
        {
            transform.position += (Vector3)(Vector2.right * horizontalSpeed * Time.deltaTime);

            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(results[0].bounds.min.x - coll.size.x, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = initialPosition;
            }
        }
        else if(horizontalSpeed < 0)
        {
            transform.position += (Vector3)(Vector2.right * horizontalSpeed * Time.deltaTime);

            if (coll.OverlapCollider(contactFilter, results) > 0)
            {
                transform.position = new Vector3(results[0].bounds.max.x + coll.size.x, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = initialPosition;
            }
        }
    }
}
