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

        if (currentPhase == null) {
            Debug.Log("STARTING PHASE 1");
            var prefab = (GameObject) Instantiate(Resources.Load("Prefabs/Phase1"), this.transform);
            currentPhase = prefab.GetComponent<Phase>();
        }

        if (currentPhase.ShouldSpawnEnemies() == true) {
             enemySpawnManager.startSpawn();
        } else {
            enemySpawnManager.stopSpawn();
        }
       
    }    
}