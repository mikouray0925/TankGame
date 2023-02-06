using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlTank : MonoBehaviour 
{
    [Header ("References")]
    [SerializeField] private CameraBase thirdPersonCam;
    [SerializeField] private CameraBase aimingCam;
    [SerializeField] private TankMovement movement;
    [SerializeField] private TurretController turret;
    [SerializeField] private TankHeadlights headlights;
    [SerializeField] private TankMinePlacer minePlacer;

    [Header ("MouseControl")]
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    bool isAiming;

    void Awake() {
        GameManager.currentCamera = thirdPersonCam;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        if (Time.timeScale == 0) return;

        GameManager.currentCamera = isAiming ? aimingCam : thirdPersonCam;

        if (isAiming) {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;
            turret.turretRotation += mouseX;
            turret.canonLiftAngle += mouseY;
            // turret.RotateTurret(mouseX);
            // turret.LiftCanon(mouseY);
        }

        if (Input.GetMouseButtonDown(0)) turret.Fire();
        if (Input.GetKeyDown(KeyCode.H)) headlights.state = !headlights.state;
        if (Input.GetKeyDown(KeyCode.C)) minePlacer.PlaceMine();

    }

    void FixedUpdate() {
        if (Time.timeScale == 0) return;

        movement.vertiInput = Input.GetAxis("Vertical");
        movement.horizInput = Input.GetAxis("Horizontal");

        isAiming = Input.GetMouseButton(1);
    }

    void LateUpdate() {
        if (!isAiming) {
            turret.AimAt(thirdPersonCam.GetCenterPosition(100f));
        }
    }
}
