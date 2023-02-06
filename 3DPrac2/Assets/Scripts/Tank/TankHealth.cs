using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHealth : Health
{
    [SerializeField] Transform brokenTanks;
    [SerializeField] GameObject brokenTank;
    [SerializeField] bool kill;

    void Update() {
        if (kill) {
            TakeDamage(100, Vector3.up);
        }
    }
    
    protected override void OnHealthBecomeZero() {
        gameObject.SetActive(false);
        if (brokenTank) {
            GameObject brok = Instantiate(brokenTank, transform.position, transform.rotation, brokenTanks);
        }
        if (gameObject.tag == "Player") {
            EndGameWindow.Pop("EndGameCanvas", "You Suck");
        }
    }
}
