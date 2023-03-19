using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSliderIconCallback : MonoBehaviour
{
    [SerializeField] private bool isMute;
    [SerializeField] private Sprite iconMute;
    [SerializeField] private Sprite iconUnmute;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        if(!isMute) {
            GetComponent<UnityEngine.UI.Image>().sprite = iconMute;
            isMute = true;
        }
        else {
            GetComponent<UnityEngine.UI.Image>().sprite = iconUnmute;
            isMute = false;
        }
    }
}
