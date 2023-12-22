using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAoeSpellTarget : AoeSpellTarget
{
    public float radius = 10f;

    protected override Vector3[] GetPoints()
    {
        var segments = 36;
        var pointCount = segments + 1;
        var points = new Vector3[pointCount];
    
        for (int i = 0; i < pointCount; i++) {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0.1f, Mathf.Cos(rad) * radius);
        }

        return points;
    }
}