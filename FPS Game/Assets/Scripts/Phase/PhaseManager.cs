using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;



public class PhaseManager : MonoBehaviourPunCallbacks {

    private EnemySpawnManager enemySpawnManager;

    [SerializeField] private Phase currentPhase;
    
    private void Start() {
        enemySpawnManager = GameObject.FindObjectOfType<EnemySpawnManager>();
        if (enemySpawnManager == null) {
            Debug.LogError("No enemySpawnManager");
        }

        enemySpawnManager.startSpawn();
    }    
}