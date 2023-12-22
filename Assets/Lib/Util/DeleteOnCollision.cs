using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.collider.isTrigger) {
            Destroy(gameObject);
        }
    }
}
