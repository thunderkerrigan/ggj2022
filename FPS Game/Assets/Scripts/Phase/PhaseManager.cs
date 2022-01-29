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


    }
    

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(this.attackCooldown);

        if (this.isAlive == false) { StopCoroutine(Attack()); yield break; }
        if (this.mode != EnemyMode.Attack) { StopCoroutine(Attack()); yield break; }

        this.Attack(garden: this.destinationGarden);

        StartCoroutine(Attack());
    }
    
}