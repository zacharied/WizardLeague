using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleAoeSpellTarget : AoeSpellTarget
{
    public float width = 4;
    public float height = 12;
    
    protected override Vector3[] GetPoints()
    {
        var points = new Vector3[4];
        points[0] = new Vector3(-width / 2, 0.1f, -height / 2);
        points[1] = new Vector3(width / 2, 0.1f, -height / 2);
        points[2] = new Vector3(width / 2, 0.1f, height / 2);
        points[3] = new Vector3(-width / 2, 0.1f, height / 2);
        return points;
    }
}
