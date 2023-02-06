using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Wheel[] leftWheels;
    [SerializeField] private Wheel[] rightWheels;

    [Header ("Ground")]
    [SerializeField] private float groundDrag;
    [SerializeField] private bool isGrounded;
    
    [Header ("Parameters")]
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float movingWheelTorque;
    [SerializeField] private float steeringWheelTorque;

    [Header ("SFX")]
    [Range (0f, 1f)]
    [SerializeField] private float movingEngineVolume;
    [SerializeField] private AudioSource movingEngineSound;

    [Header ("Inputs")]
    public float vertiInput;
    public float horizInput;
    
    private Rigidbody rbody;

    void Awake() {
        rbody = GetComponent<Rigidbody>();
    }

    void Update() {
        CheckGrounded();
        if (isGrounded && (vertiInput == 0) && (horizInput == 0) && 
            rbody.velocity.magnitude < 0.5f) rbody.drag = groundDrag;
        else rbody.drag = 0;

        movingEngineSound.volume = movingEngineVolume * AudioManager.EffectVolume;
        if ((vertiInput != 0) || (horizInput != 0)) {
            if (!movingEngineSound.isPlaying) movingEngineSound.Play();
        }
        else {
            movingEngineSound.Stop();
        }
    }

    void FixedUpdate() {
        float wheelTorque = 0;
        if ((vertiInput > 0 && rbody.velocity.magnitude <  maxMoveSpeed) ||
            (vertiInput < 0 && rbody.velocity.magnitude > -maxMoveSpeed)) {
            wheelTorque  += movingWheelTorque * vertiInput;
        }

        foreach (Wheel wheel in leftWheels) {
            if (vertiInput >= 0) wheel.SetTorque(wheelTorque + horizInput * steeringWheelTorque);
            else                 wheel.SetTorque(wheelTorque - horizInput * steeringWheelTorque);
        }
        foreach (Wheel wheel in rightWheels) {
            if (vertiInput >= 0) wheel.SetTorque(wheelTorque - horizInput * steeringWheelTorque);
            else                 wheel.SetTorque(wheelTorque + horizInput * steeringWheelTorque);
        }
    }

    void CheckGrounded() {
        isGrounded = false;
        foreach (Wheel wheel in leftWheels) {
            if (wheel.IsGrounded()) {
                isGrounded = true;
                break;
            }
        }
        foreach (Wheel wheel in rightWheels) {
            if (wheel.IsGrounded()) {
                isGrounded = true;
                break;
            }
        }
    }

    public Vector3 velocity {
        get {
            return rbody.velocity;
        }
        private set {}
    }
}
