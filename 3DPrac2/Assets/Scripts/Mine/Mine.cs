using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header ("Ground")]
    [SerializeField] Transform groundPoint;
    [SerializeField] LayerMask groundLayers;

    [Header ("Trigger")]
    [SerializeField] Transform triggerPoint;
    [SerializeField] LayerMask triggerLayers;

    [Header ("Explosion")]
    [SerializeField] MineExplosion explosion;
    [SerializeField] float explosionDelay;

    public bool setted {get; private set;}
    public bool triggered {get; private set;}
    public bool exploded {get; private set;}

    MeshRenderer mrenderer;
    BoxCollider bcollider;
    Rigidbody rbody;

    void Awake() {
        mrenderer = GetComponent<MeshRenderer>();
        bcollider = GetComponent<BoxCollider>();
        rbody = GetComponent<Rigidbody>();
    }

    void Update() {
        if (!setted && IsGrounded()) {
            rbody.isKinematic = true;
            setted = true;
        }

        if (!setted) return; 

        if (!triggered && IsTriggered()) {
            Invoke(nameof(Explode), explosionDelay);
            triggered = true;
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(groundPoint.position, -groundPoint.up, 0.1f, groundLayers);
    }

    bool IsTriggered() {
        return Physics.Raycast(triggerPoint.position, triggerPoint.up, 1f, triggerLayers);
    }

    public void Explode() {
        if (exploded) return;
        mrenderer.enabled = false;
        bcollider.enabled = false;
        explosion.Play();
        exploded = true;
    }

    public void Disappear() {
        mrenderer.enabled = false;
        bcollider.enabled = false;
    }
}
