using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBase : MonoBehaviour
{
    public Vector3 lookForward   {get; protected set;}
    public Vector3 lookForwardXZ {get; protected set;}
    public Vector3 lookRightXZ   {get; protected set;}

    protected virtual void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected void ComputeDirections() {
        lookForward   = lookForward.normalized;
        lookForwardXZ = new Vector3(lookForward.x, 0, lookForward.z);
        lookForwardXZ = lookForwardXZ.normalized;
        lookRightXZ   = Vector3.Cross(Vector3.up, lookForwardXZ);
    }

    public Vector3 GetCenterPosition(float distance) {
        return transform.position + lookForward * distance;
    }

    public void MoveTo(Transform target) {
        transform.position = target.position;
    }

    public void LookAt(Transform target) {
        transform.LookAt(target);
    }

    public void LookAt(Transform target, Vector3 up) {
        transform.LookAt(target, up);
    }

    public void SetActive(bool val) {
        gameObject.SetActive(val);
    }

    public Transform camTransform {
        get {
            return transform;
        }
        private set {}
    }
}
