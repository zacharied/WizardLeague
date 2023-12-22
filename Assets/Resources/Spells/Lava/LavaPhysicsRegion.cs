using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPhysicsRegion : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GameBall"))
        {
            Debug.Log("Ball collision");
            var effectBounds = GetComponent<Collider>().bounds;
            var floor = effectBounds.min.y;
            var heightProportion = (other.bounds.center.y - floor) / effectBounds.size.y;
            if (other.GetComponent<Rigidbody>().velocity.y < 0)
            {
                other.GetComponent<Rigidbody>().AddForce(0f, -Physics.gravity.y * other.attachedRigidbody.mass * 2, 0f);
            }
            other.GetComponent<Rigidbody>().AddForce(0f, (-Physics.gravity.y * other.attachedRigidbody.mass * 1.5f) * ((1 - heightProportion) * 0.5f + 0.5f), 0f);
        }
    }
}
