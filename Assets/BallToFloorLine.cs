using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallToFloorLine : MonoBehaviour
{
    private const float maskExtraWidth = 10f;
    
    public Ball ball;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var rectTransform = GetComponent<RectTransform>();

        Debug.Log(ball.boundingBox.xMin);
        rectTransform.position = new Vector3(
            ball.boundingBox.xMin,
            Camera.main.WorldToScreenPoint(new Vector3(0, ball.floorRaycast.worldFloorY)).y);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ball.boundingBox.width);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ball.boundingBox.height);
    }
}
