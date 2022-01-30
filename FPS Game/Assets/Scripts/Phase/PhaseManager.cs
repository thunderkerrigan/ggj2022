using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;


public class PhaseManager : MonoBehaviourPunCallbacks {

    private EnemySpawnManager enemySpawnManager;
    private WeaponDropManager weaponDropManager;

    [SerializeField] private Phase currentPhase;

    private Timer timer;
    
    private void Start() {
        enemySpawnManager = GameObject.FindObjectOfType<EnemySpawnManager>();
        if (enemySpawnManager == null) {
            Debug.LogError("No enemySpawnManager");
        }

        weaponDropManager = GameObject.FindObjectOfType<WeaponDropManager>();
        if (weaponDropManager == null) {
            Debug.LogError("No weaponDropManager");
        }

        if (currentPhase == null) {
            Debug.Log("STARTING PHASE 1");
            var prefab = (GameObject) Instantiate(Resources.Load("Prefabs/Phase1"), this.transform);
            currentPhase = prefab.GetComponent<Phase>();
        }

        timer = GetComponent<Timer>();
        if (timer == null) {
            Debug.LogError("TIMER NOT FOUND");
        }

        this.newPhaseStarted();

        // Start to drop weapons directly
        weaponDropManager.startSpawn();
    }

    private void Update() {
        if (this.currentPhase.hasTimer() && this.timer.isTimerFinished()) {
            Debug.Log("TIMER FINISHED");
            // SWITCH TO PHASE 2
            if (currentPhase.identifier == "PHASE_1") {
                var prefab = (GameObject) Instantiate(Resources.Load("Prefabs/Phase2"), this.transform);
                currentPhase = prefab.GetComponent<Phase>();
                this.newPhaseStarted();
            }
        }
    }

    private void newPhaseStarted() {
        if (currentPhase.ShouldSpawnEnemies() == true) {
             enemySpawnManager.startSpawn();
        } else {
            enemySpawnManager.stopSpawn();
            this.killAllEnemies();
        }

        if (currentPhase.hasTimer()) {
            timer.startTimer(currentPhase.MaxTimer());
        } else {
            timer.stop();
        }

        var players = GameObject.FindObjectsOfType<IsoPlayerController>();
        if (currentPhase.hasPVPEnabled()) {
            foreach (IsoPlayerController player in players) {
                player.canTakeDamage = true;
            }
        } else {
            foreach (IsoPlayerController player in players) {
                player.canTakeDamage = false;
            }
        }
    }

    private void killAllEnemies() {
        var enemies = FindObjectsOfType(typeof(Enemy));
        foreach (Enemy enemy in enemies) {
            enemy.TakeDamage(1000f);
        }
    }
}