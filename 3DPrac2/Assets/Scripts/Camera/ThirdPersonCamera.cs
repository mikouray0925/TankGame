using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : CameraBase
{
    [SerializeField] protected Transform target;

    void Update() {
        lookForward = transform.forward;
        ComputeDirections();
    }
}
