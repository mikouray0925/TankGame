using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{   
    [Header ("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float flyingTime; 
    [SerializeField] private float lifeTime;

    [Header ("VFX")]
    [SerializeField] public MissileExplosion explosionVFX;

    private Vector3 selfVelocity;
    private Vector3 velocity;
    private bool exploded;

    MeshRenderer mrenderer;
    Rigidbody rbody;
    BoxCollider bcollider;

    float spawnTime;

    void Awake() {
        mrenderer = GetComponent<MeshRenderer>();
        rbody = GetComponent<Rigidbody>();
        bcollider = GetComponent<BoxCollider>();

        gameObject.SetActive(false);
    }

    void Update() {
        if (Time.time - spawnTime > lifeTime && !explosionVFX.isPlaying) {
            print("Missile life time over.");
            Deactivate();
            return;
        }

        if (!rbody.useGravity) {
            rbody.velocity = velocity;
        }
        else {
            rbody.velocity = new Vector3(velocity.x, rbody.velocity.y, velocity.z);
        }
        transform.forward = new Vector3(selfVelocity.x, rbody.velocity.y, selfVelocity.z);
    }

    public void Launch(Vector3 baseVelocity, Transform firePnt, bool isAiming) {
        gameObject.SetActive(true);
        transform.position = firePnt.position;
        transform.forward  = firePnt.forward;
        selfVelocity = firePnt.forward * speed;
        velocity = baseVelocity + selfVelocity;
        
        mrenderer.enabled = true;
        bcollider.enabled = true;
        rbody.isKinematic = false;
        rbody.useGravity = false;

        //Aiming Damage Multiplier
        if(isAiming){
            explosionVFX.DamageSet(3f,0.9f);
        }
        else {
            explosionVFX.DamageSet(2f,0.6f);
        }

        Invoke(nameof(StartFalling), flyingTime); 

        exploded = false;
        spawnTime = Time.time;
    }

    void StartFalling() {
        rbody.useGravity = true;
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision) {
        if (!exploded) {
            explosionVFX.forward = collision.contacts[0].normal;
            explosionVFX.Play();
            
            mrenderer.enabled = false;
            bcollider.enabled = false;
            rbody.isKinematic = true;

            exploded = true;
        }
    }
}
