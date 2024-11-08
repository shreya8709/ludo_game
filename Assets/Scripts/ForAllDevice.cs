using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAllDevice : MonoBehaviour
{
    public float sceneWidth = 10;
    Camera _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }
    void Update()
    {
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        _camera.orthographicSize = desiredHalfHeight;
    }
}
