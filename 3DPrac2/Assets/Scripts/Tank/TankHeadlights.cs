using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHeadlights : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] GameObject leftLight;
    [SerializeField] GameObject rightLight;

    public bool state {
        get {
            return leftLight.activeSelf || rightLight.activeSelf;
        }
        set {
            leftLight.SetActive(value);
            rightLight.SetActive(value);
        }
    }
}
