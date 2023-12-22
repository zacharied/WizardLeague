using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellTargetComponent : MonoBehaviour
{
    private void Start()
    {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        transform.eulerAngles += Vector3.forward * 23 * Time.deltaTime;
    }

    private void UpdatePosition()
    {
        var ball = GameObject.FindGameObjectWithTag("PlayerOnField_P1");
        transform.position = Camera.main.WorldToScreenPoint(ball.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
