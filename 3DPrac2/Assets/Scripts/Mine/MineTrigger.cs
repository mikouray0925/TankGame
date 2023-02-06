using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrigger : MonoBehaviour
{   
    [SerializeField] Mine mine;
    [SerializeField] MineExplosion explosion;
    [SerializeField] float explosionDelay;
    [SerializeField] bool exploded;

    void OnCollisionEnter(Collision other) {
        print("Mine collided");
        print(other.gameObject.tag);
        if (!mine.setted || exploded) return;
        if (other.gameObject.tag == "Tank") {
            Invoke(nameof(Explode), explosionDelay);
        }
    }

    void Explode() {
        mine.Disappear();
        explosion.Play();
        exploded = true;
    }
}
