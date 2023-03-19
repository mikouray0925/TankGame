using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform tank;
    [SerializeField] private Transform canon;
    [SerializeField] private AudioSource canonAudioSource;

    [Header ("Camera")]
    [SerializeField] private CameraBase canonCam;
    [SerializeField] private Transform canonCamPoint;
    [SerializeField] private Transform canonCamCenter;
    
    [Header ("Attack")]
    [SerializeField] private float damage = 2f;
    [SerializeField] public  float damageMultiplier = 1f;
    [SerializeField] private float maxCanonAngle;
    [SerializeField] private MissilePool missilePool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCD;
    [SerializeField] private bool fireReady;

    [Header ("SFX")]
    [Range (0f, 1f)]
    [SerializeField] private float fireVolume;
    [SerializeField] private AudioClip fireSFX;
    [Range (0f, 1f)]
    [SerializeField] private float reloadVolume;
    [SerializeField] private AudioClip reloadSFX;

    [Header ("VFX")]
    [SerializeField] private ParticleSystem fireVFX;
    
    void Awake() {
        Invoke(nameof(ResetFire), 3f);
    }

    void Update() {
        
    }

    void LateUpdate() {
        if (canonCam) {
            canonCam.MoveTo(canonCamPoint);
            canonCam.LookAt(canonCamCenter, Vector3.up);
        }
    }

    public void Fire(bool isAiming) {
        if (fireReady) {
            Missile missile = missilePool.GetMissile();
            missile.explosionVFX.damage = damage * damageMultiplier;
            missile.Launch(tank.GetComponent<Rigidbody>().velocity, firePoint, isAiming);
            missilePool.GetMissile().Launch(tank.GetComponent<Rigidbody>().velocity, firePoint, isAiming);
            fireVFX.Clear();
            fireVFX.Play();
            canonAudioSource.PlayOneShot(fireSFX, fireVolume * AudioManager.EffectVolume);
            fireReady = false;
            Invoke(nameof(ResetFire), fireCD);
        }
    }

    void ResetFire() {
        canonAudioSource.PlayOneShot(reloadSFX, reloadVolume * AudioManager.EffectVolume);
        fireReady = true;
    }

    public void AimAt(Transform target) {
        AimAt(target.position);
    }

    public void AimAt(Vector3 pos) {
        /** Rotate the turret plane **/

        // let the turret face the front side of the tank
        turretRotation = 0; 
        // transfer the target position from world space to turret space
        Vector3 relativePos = pos - transform.position;
        Vector3 turretPlanePos = Vector3.ProjectOnPlane(relativePos, transform.up);
        // Note: Vector3.Angle return positive value
        float angle = Vector3.Angle(transform.forward, turretPlanePos);
        // use cross to check whether the target is on the left side or right side
        if (Vector3.Cross(transform.forward, turretPlanePos).y >= 0) {
            turretRotation = angle;
        }
        else {
            turretRotation = -angle;
        }
        
        /** Lift the canon **/

        // reset to zero because maybe we don't need to lift it up.
        canonLiftAngle = 0;
        // transfer the target position from world space to cannon space
        relativePos = pos - canon.position;
        // lift the canon only when the target is above the canon.
        if (Vector3.Dot(relativePos, transform.up) > 0) {   
            Vector3 canonPlanePos = Vector3.ProjectOnPlane(relativePos, canon.right);
            canonLiftAngle = Vector3.Angle(canon.forward, canonPlanePos);
        }     
    }

    public Vector3 firePosition {
        set {
            AimAt(value);
        }
        get {
            return firePoint.position;
        }
    }

    public Vector3 fireDirection {
        set {
            AimAt(firePoint.position + value);
        }
        get {
            return firePoint.forward;
        }
    }

    public float turretRotation  {
        set {
            transform.localRotation = Quaternion.Euler(0, value, 0);
        }
        get {
            if (transform.localEulerAngles.x != 0 || transform.localEulerAngles.z != 0) {
                transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
            }
            return transform.localEulerAngles.y;
        }
    }

    public float canonLiftAngle {
        set {
            float angle = Mathf.Clamp(-value, -maxCanonAngle, 0);
            canon.localRotation = Quaternion.Euler(angle, 0, 0);
        }
        get {
            return -ToSignedEularAngle(canon.localEulerAngles.x);
        }
    }

    float ToSignedEularAngle(float eularAngle) {
        while (eularAngle >= 360f) eularAngle -= 360f;
        if (eularAngle >= 0 && eularAngle <= 180) {
            return eularAngle;
        }
        else {
            return -(360f - eularAngle);
        }
    }


    // legacy    
    public void SetTurretRotation(float angle) {
        transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    // legacy    
    public void RotateTurret(float delta) {
        transform.Rotate(0, delta, 0);
    }

    // legacy    
    public void SetCanonLiftAngle(float angle) {
        angle = Mathf.Clamp(-angle, -maxCanonAngle, 0);
        canon.localRotation = Quaternion.Euler(angle, 0, 0);
    }

    // legacy    
    public void LiftCanon(float delta) {
        canon.Rotate(-delta, 0, 0);
        float angle = ToSignedEularAngle(canon.localEulerAngles.x);
        if (angle > 0.1f) canon.localRotation = Quaternion.Euler(0, 0, 0);
        if (angle < -maxCanonAngle) canon.localRotation = Quaternion.Euler(-maxCanonAngle, 0, 0);
    }

    
}
