using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    

    void LateUpdate() {
        transform.LookAt(transform.position + GameManager.currentCamera.camTransform.forward);
    }
}
