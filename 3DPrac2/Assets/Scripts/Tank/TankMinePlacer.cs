using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMinePlacer : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform mineHolder;
    [SerializeField] GameObject mine;
    [SerializeField] float mineCD;

    bool mineReady = true;

    public void PlaceMine() {
        if (!mineReady) return;
        Instantiate(mine, spawnPoint.position, spawnPoint.rotation, mineHolder);
        mineReady = false;
        Invoke(nameof(ResetMine), mineCD);
    }

    void ResetMine() {
        mineReady = true;
    }
}
