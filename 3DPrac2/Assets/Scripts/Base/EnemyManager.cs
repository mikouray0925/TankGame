using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    void Update() {
        /*
        if (GetAliveNum() <= 0) {
            EndGameWindow.Pop("EndGameCanvas", "You Win");
        }
        */
    }

    public int GetAliveNum() {
        int num = 0;
        foreach (Transform child in transform) {
            if (child.TryGetComponent<Health>(out Health health)) {
                if (health.IsAlive()) num++;
            }
        }
        return num;
    }
}
