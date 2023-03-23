using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform CameraTransform;
    [SerializeField] private float SmoothTimePosition = 0.3f;
    [SerializeField] private float SmoothTimeRotation = 3.0f;

    private Vector3 Velocity = Vector3.zero;

    void Update()
    {
        Vector3 TargetPosition = CameraTransform.position;
        Quaternion TargetRotation = CameraTransform.rotation;

        transform.position = Vector3.SmoothDamp(this.transform.position, TargetPosition, ref Velocity, SmoothTimePosition);
        transform.rotation = Quaternion.Lerp(this.transform.rotation, TargetRotation, SmoothTimeRotation * Time.deltaTime);
    }
}
