using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
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

    public void RespawnEnemies() {
        foreach (Transform child in transform){
            Destroy(child.gameObject);
        }
        for(int i =0;i<3;i++){
            Vector3 randPos = new Vector3(Random.Range(150f, 50f), 5f, Random.Range(150f, 50f));
            NavMesh.SamplePosition(randPos, out NavMeshHit hit, 64f, 1);
            Vector3 pos = hit.position;
            pos.y = 3f;
            GameObject newtank = Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
            newtank.GetComponentInChildren<TurretController>().missilePool = GameObject.Find("MissilePool_Blue").GetComponent<MissilePool>();
            newtank.TryGetComponent<Health>(out Health health);
            health.SetMaxHealth(Mathf.RoundToInt(10 * ( GameManager.currentLevel * 0.5f + 1)));
            health.Heal();
        }
    }
}
