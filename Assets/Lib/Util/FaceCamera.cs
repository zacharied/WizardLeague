using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FaceCamera : MonoBehaviour
{
    public bool XAxis;

    public bool YAxis;

    public bool ZAxis;
    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(new Vector3());    
    }
}
