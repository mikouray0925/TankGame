using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : CameraBase
{
    [SerializeField] private Transform eyePoint;

    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    float rotationX = 0;
    float rotationY = 0;

    void Update() {
        transform.position = eyePoint.position;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        rotationX -= mouseY;
        rotationY += mouseX;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);
        
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        //lookForward = transform.rotation * Vector3.forward;
        lookForward = transform.forward;
        ComputeDirections();
    }
}
