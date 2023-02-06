using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareScene : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void start() {
        gameManager.BackToMainMenu();
    }
}
