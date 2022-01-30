using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;

public delegate void DefeatHandler(string reason);

public class PhaseManager : MonoBehaviourPunCallbacks {

    public event DefeatHandler OnDefeatHandler;

    private WeaponDropManager weaponDropManager;

    private Phase currentPhase;
    private Coroutine currentPhaseCoroutine;

    [SerializeField] private Phase[] phases;

    private Timer timer;
    
    private void Start() {
        weaponDropManager = GameObject.FindObjectOfType<WeaponDropManager>();
        if (weaponDropManager == null) {
            Debug.LogError("No weaponDropManager");
        }

        // Start to drop weapons directly
        weaponDropManager.startSpawn();
    }

    public void StopGame()
    {
        if (currentPhaseCoroutine != null)
        {
            StopCoroutine(currentPhaseCoroutine);
        }
    }
    
    public void StartGame()
    {
        if (currentPhaseCoroutine != null)
        {
            StopCoroutine(currentPhaseCoroutine);
        }

        currentPhaseCoroutine = StartCoroutine(deployPhaseCoroutine());
    }

    private IEnumerator deployPhaseCoroutine()
    {
        foreach (Phase phase in phases)
        {
            var players = GameObject.FindObjectsOfType<IsoPlayerController>();
            foreach (IsoPlayerController player in players)
                {
                    // TODO: handle with photon
                    player.canTakeDamage = phase._pvpEnabled;
                }
            if (phase.ShouldSpawnEnemies()) {
                EnemySpawnManager.Instance.startSpawn();
            } else {
                EnemySpawnManager.Instance.stopSpawn();
                this.killAllEnemies();
            }
            yield return new WaitForSeconds(phase.MaxTimer());
        }

        if (OnDefeatHandler != null)
        {
            OnDefeatHandler("You have been defeated");
        }
    }

    private void killAllEnemies() {
        var enemies = FindObjectsOfType(typeof(Enemy));
        foreach (Enemy enemy in enemies) {
            enemy.TakeDamage(1000f);
        }
    }
}