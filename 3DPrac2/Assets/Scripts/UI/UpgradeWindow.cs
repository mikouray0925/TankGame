using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    // Start is called before the first frame update
    public void Show(){
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Hide(){
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
