using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GetFloorRaycast))]
[RequireComponent(typeof(Get2DBoundingBox))]
public class Ball : MonoBehaviour
{
    internal Get2DBoundingBox boundingBox;
    internal GetFloorRaycast floorRaycast;

    private void Start()
    {
        boundingBox = GetComponent<Get2DBoundingBox>();
        floorRaycast = GetComponent<GetFloorRaycast>();
    }

    private void OnCollisionEnter(Collision other)
    {
        var parentSpell = other.collider.GetComponentInParent<ISpellCastable>();
    }
}
