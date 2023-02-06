using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePool : MonoBehaviour
{
    [SerializeField] private int num;
    [SerializeField] private GameObject missilePrefab;   

    void Awake () {
        for (int i = 0; i < num; i++) {
            Instantiate(missilePrefab, transform);
        }
    }

    public Missile GetMissile() {
        foreach (Transform child in transform) {
            if (!child.gameObject.activeSelf) {
                return child.GetComponent<Missile>();
            }
        }
        return Instantiate(missilePrefab, transform).GetComponent<Missile>();
    }
}
