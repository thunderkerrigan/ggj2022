using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;

public delegate void DefeatHandler(string reason);

public class PhaseManager : MonoBehaviourPunCallbacks
{

    public event DefeatHandler OnDefeatHandler;

    private WeaponDropManager weaponDropManager;

    private Phase currentPhase;
    private Coroutine currentPhaseCoroutine;
    private Coroutine checkVictoryOrDefeatConfitionCoroutine;

    [SerializeField] private Phase[] phases;

    private Timer timer;

    private Garden[] gardens;

    private IsoPlayerController[] players;

    private void Start()
    {
        weaponDropManager = GameObject.FindObjectOfType<WeaponDropManager>();
        if (weaponDropManager == null)
        {
            Debug.LogError("No weaponDropManager");
        }

        gardens = GameObject.FindObjectsOfType<Garden>();

        // Start to drop weapons directly
        weaponDropManager.startSpawn();
    }

    public void StopGame()
    {
        if (currentPhaseCoroutine != null)
        {
            StopCoroutine(currentPhaseCoroutine);
        }

        if (checkVictoryOrDefeatConfitionCoroutine != null) {
            StopCoroutine(checkVictoryOrDefeatConfitionCoroutine);
        } 
    }

    public void StartGame()
    {

        players = GameObject.FindObjectsOfType<IsoPlayerController>();

        if (currentPhaseCoroutine != null)
        {
            StopCoroutine(currentPhaseCoroutine);
        }

        if (checkVictoryOrDefeatConfitionCoroutine != null) {
            StopCoroutine(checkVictoryOrDefeatConfitionCoroutine);
        } 

        currentPhaseCoroutine = StartCoroutine(deployPhaseCoroutine());
        checkVictoryOrDefeatConfitionCoroutine = StartCoroutine(checkVictoryOrDefeatCondition());
    }

    private IEnumerator checkVictoryOrDefeatCondition() {

        yield return new WaitForSeconds(1);
       
        if (currentPhase.identifier == "PHASE_1")
        {

            // if (players.Length == 1) {
            //     players = GameObject.FindObjectsOfType<IsoPlayerController>();
            // }

             // Check defeat condition
            var oneGardenAlive = false;
            foreach (Garden garden in gardens)
            {
                // TODO: Photon?
                if (garden.isAlive())
                {
                    oneGardenAlive = true;
                    break;
                }
            }

            if (oneGardenAlive == false)
            {
                OnDefeatHandler("All Gardens were eaten");
                StopCoroutine(checkVictoryOrDefeatConfitionCoroutine);
                yield return null;
            }
        } else if (currentPhase.identifier == "PHASE_2") {
            // if (players.Length > 1) {
            //     foreach (IsoPlayerController player in players) {
            //         if (player.isAlive == false) {
            //             if (player.isCurrentPlayer()) {
            //                 // DEFEAT
            //                 OnDefeatHandler("You're dead");
            //                 StopCoroutine(checkVictoryOrDefeatConfitionCoroutine);
            //                 yield return null;
            //             } else {
            //                 // VICTORY
            //                 OnDefeatHandler("Victory!");
            //                 StopCoroutine(checkVictoryOrDefeatConfitionCoroutine);
            //                 yield return null;
            //             }
            //         }
            //     }
            // } else {
            //     // VICTORY
            //     OnDefeatHandler("You're alone");
            //     StopCoroutine(checkVictoryOrDefeatConfitionCoroutine);
            //     yield return null;
            // }
        }

        checkVictoryOrDefeatConfitionCoroutine = StartCoroutine(checkVictoryOrDefeatCondition());
        yield return null;
    }

    private IEnumerator deployPhaseCoroutine()
    {
        foreach (Phase phase in phases)
        {
            currentPhase = phase;
            foreach (IsoPlayerController player in players)
            {
                // TODO: handle with photon
                player.canTakeDamage = phase._pvpEnabled;
            }
            if (phase.ShouldSpawnEnemies())
            {
                EnemySpawnManager.Instance.startSpawn();
            }
            else
            {
                EnemySpawnManager.Instance.stopSpawn();
                this.killAllEnemies();
            }
            yield return new WaitForSeconds(phase.MaxTimer());
        }

        // if (OnDefeatHandler != null)
        // {
        //     OnDefeatHandler("You have been defeated");
        // }
    }

    private void killAllEnemies()
    {
        var enemies = FindObjectsOfType(typeof(Enemy));
        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(1000f);
        }
    }
}