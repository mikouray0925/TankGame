using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform mesh;
    [SerializeField] private WheelCollider wcollider;

    [Header ("Ground")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform groundCheckPoint;

    [Header ("VFX")]
    [SerializeField] private GameObject[] dirtSplatters;

    void Update() {
        UpdateTransform();

        bool isGrounded = IsGrounded();
        foreach (GameObject dirtSplatter in dirtSplatters) {
            dirtSplatter.SetActive(isGrounded && (wcollider.motorTorque > 0));
        }   
    }

    void UpdateTransform() {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        wcollider.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }

    public void SetTorque(float torque) {
        if (Mathf.Abs(torque) > 0) wcollider.motorTorque = torque;
        else wcollider.brakeTorque = 0;
    }

    public void Brake() {
        wcollider.brakeTorque = 0;
    }

    public bool IsGrounded() {
        return Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.1f, groundLayers);
    }
}
