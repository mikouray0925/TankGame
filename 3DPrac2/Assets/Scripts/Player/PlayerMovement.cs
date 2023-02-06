using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool isGrounded;

    [Header ("References")]
    [SerializeField] private Transform head;
    [SerializeField] private CameraBase cam;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayers;

    [Header ("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float maxGroundSpeed;
    [SerializeField] private float maxAirSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCD;
    [SerializeField] private bool readyToJump;
    [SerializeField] private float maxSlopeAngle;

    private Rigidbody rbody;

    void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rbody = GetComponent<Rigidbody>();
    }

    void Update() {
        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.2f, groundLayers);
        if (isGrounded) {
            rbody.drag = groundDrag;
            LimitVelocityTo(maxGroundSpeed);
        }
        else {
            rbody.drag = 0;
            LimitFlatVelocityTo(maxAirSpeed);
        }

        head.forward = cam.lookForward;

        if (Input.GetKeyDown("space") && readyToJump) {
            rbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCD);
        }
    }

    void FixedUpdate() {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
            Vector3 moveForward = Input.GetAxis("Vertical")   * cam.lookForwardXZ + 
                                  Input.GetAxis("Horizontal") * cam.lookRightXZ;
            if (isGrounded) {
                rbody.AddForce(GetMoveDirectionOnGround(moveForward) * speed * 10f, ForceMode.Force);
            }
            else {
                rbody.AddForce(moveForward * speed * 10f * airMultiplier, ForceMode.Force);
            }
            transform.forward = moveForward;
        }
    }

    Vector3 GetMoveDirectionOnGround(Vector3 directionXZ) {
        RaycastHit groundHit;
        if (Physics.Raycast(groundCheckPoint.position, Vector3.down, out groundHit, 0.3f, groundLayers)) {
            if (Vector3.Angle(Vector3.up, groundHit.normal) > maxSlopeAngle) {
                return directionXZ;
            }
            return Vector3.ProjectOnPlane(directionXZ, groundHit.normal).normalized;
        }
        return directionXZ;
    }

    void LimitVelocityTo(float maxSpeed) {
        if (rbody.velocity.magnitude > maxSpeed) {
            rbody.velocity = rbody.velocity.normalized * maxSpeed;
        }
    }

    void LimitFlatVelocityTo(float maxSpeed) {
        Vector3 velocityXZ = new Vector3(rbody.velocity.x, 0, rbody.velocity.z);
        if (velocityXZ.magnitude > maxSpeed) {
            velocityXZ = velocityXZ.normalized * maxSpeed;
        }
        rbody.velocity = new Vector3(velocityXZ.x, rbody.velocity.y, velocityXZ.z);
    }

    void ResetJump() {
        readyToJump = true;
    }
}
