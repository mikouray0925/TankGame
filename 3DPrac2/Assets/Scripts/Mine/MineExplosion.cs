using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : Explosion
{
    [SerializeField] GameObject mine;

    void OnParticleSystemStopped() {
        Destroy(mine);
    }
}
