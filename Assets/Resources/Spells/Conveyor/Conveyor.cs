using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        OnCollision(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnCollision(other);
    }

    private void OnCollision(Collider other)
    {
        if (other.CompareTag("GameBall")) {
            other.GetComponent<Rigidbody>().angularVelocity += new Vector3(20, 0, 0);
        }
    }
}
