using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFloorRaycast : MonoBehaviour
{
    internal float worldFloorY;
    
    // Update is called once per frame
    void Update()
    {
        var ray = new Ray(gameObject.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hit)) {
            worldFloorY = hit.point.y;
        }
    }
}
