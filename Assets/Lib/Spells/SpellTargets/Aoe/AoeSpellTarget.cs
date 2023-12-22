using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class AoeSpellTarget : MonoBehaviour, ISpellTarget
{
    private LineRenderer line;

    public void DrawTarget()
    {
        line = GameObject.FindGameObjectWithTag("WorldSpaceCursor").GetComponent<LineRenderer>();
        line.enabled = true;
        var points = GetPoints();
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    public void ClearTarget()
    {
        line.enabled = false;
    }

    protected abstract Vector3[] GetPoints();
}
