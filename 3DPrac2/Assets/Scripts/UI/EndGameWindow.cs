using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameWindow : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;

    public void Show(string message) {
        panel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        text.text = message;
    }

    public void Hide() {
        panel.SetActive(false);
    }

    public static bool Pop(string objectName, string message) {
        EndGameWindow endGameWindow = EndGameWindow.FindInstance(objectName);
        if (endGameWindow) {
            endGameWindow.Show(message);
            return true;
        }
        print("Cannot find end game window.");
        return false;
    }

    public static EndGameWindow FindInstance(string objectName) {
        GameObject obj = GameObject.Find(objectName);
        if (!obj) return null;
        return obj.transform.GetComponent<EndGameWindow>();
    }
}
