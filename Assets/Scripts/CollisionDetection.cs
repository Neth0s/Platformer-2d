using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    BoxCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        var contactFilter = new ContactFilter2D().NoFilter();
        var results = new List<Collider2D>();
        coll.OverlapCollider(contactFilter, results);

        Debug.Log(results.Count);
    }
}
