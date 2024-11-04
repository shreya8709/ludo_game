using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Range(0, 5)] public int rotateSpeed;
    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotateSpeed);
    }
}
