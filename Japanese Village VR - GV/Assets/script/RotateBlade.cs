using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlade : MonoBehaviour
{
    // Speed of rotation
    public float rotationSpeed = 50f;

    void Update()
    {
        // Rotate the blade around X axis (horizontal rotation)
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}