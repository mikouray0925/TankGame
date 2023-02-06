using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : Explosion
{
    [Header ("Reference")]
    [SerializeField] private Missile missile;
    
    void OnParticleSystemStopped() {
        missile.Deactivate();
    }
}
