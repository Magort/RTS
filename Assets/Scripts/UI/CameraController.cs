using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(1, 10)] public float intensity;
    bool dragging;
    Vector3 lastMousePos = new();

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragging = true;
            lastMousePos = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(1))
            dragging = false;

        if (!dragging)
            return;

        Camera.main.transform.position
            -= (new Vector3(Input.mousePosition.x - lastMousePos.x, 0, Input.mousePosition.y - lastMousePos.y) / 100 )
            * intensity;
        lastMousePos = Input.mousePosition;
    }
}
