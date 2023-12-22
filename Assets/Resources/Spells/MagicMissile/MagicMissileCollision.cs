using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissileCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameBall")) {
            other.GetComponent<Rigidbody>().AddExplosionForce(10000f, transform.position, 100f);
        }
    }
}
