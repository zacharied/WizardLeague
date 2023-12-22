using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get2DBoundingBox : MonoBehaviour
{
    private MeshFilter mesh;
    private Camera camera;

    internal float xMin;
    internal float xMax;
    internal float yMin;
    internal float yMax;

    internal float width => xMax - xMin;
    internal float height => yMax - yMin;
    
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        xMin = float.MaxValue;
        xMax = float.MinValue;
        yMin = float.MaxValue;
        yMax = float.MinValue;
        
        foreach (var vertex in mesh.mesh.vertices) {
            var screenSpace = camera.WorldToScreenPoint(mesh.transform.TransformPoint(vertex));
            xMin = Mathf.Min(xMin, screenSpace.x);
            xMax = Mathf.Max(xMin, screenSpace.x);
            yMin = Mathf.Min(yMin, screenSpace.y);
            yMax = Mathf.Max(yMax, screenSpace.y);
        }
    }
}
