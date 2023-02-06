using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header ("Damage")]
    [SerializeField] float damage;
    [SerializeField] float damageRadius;
    [SerializeField] float causeDamageDelay;
    [SerializeField] float force;

    [Header ("SFX")]
    [Range (0f, 1f)]
    [SerializeField] float volume;
    [SerializeField] AudioClip explosionSFX;
    
    ParticleSystem explosionVFX;
    AudioSource audioSource;

    void Awake() {
        explosionVFX = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Play() {
        explosionVFX.Clear();
        explosionVFX.Play();
        audioSource.PlayOneShot(explosionSFX, volume * AudioManager.EffectVolume);
        Invoke(nameof(CauseDamage), causeDamageDelay);
    }

    void CauseDamage() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        HashSet<Health> healthSet = new HashSet<Health>();
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent(out DamageablePart damageable)) {
                healthSet.Add(damageable.health);
            }
            if (collider.TryGetComponent(out Rigidbody rbody)) {
                Vector3 damageDir = (transform.position - collider.transform.position).normalized;
                rbody.AddForce(-damageDir * force, ForceMode.Impulse);
            }
        }
        foreach (Health health in healthSet) {
                Vector3 damageDir = (transform.position - health.transform.position).normalized;
                health.TakeDamage(damage, damageDir);
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, damageRadius);    
    }

    public bool isPlaying {
        get {
            return explosionVFX.isPlaying;
        }
        private set {}
    }

    public Vector3 forward {
        get {
            return transform.forward;
        }
        set {
            transform.forward = value;
        }
    }
}
