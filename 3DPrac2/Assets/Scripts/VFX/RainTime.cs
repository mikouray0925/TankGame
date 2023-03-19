using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Random.Range(30, 120));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
