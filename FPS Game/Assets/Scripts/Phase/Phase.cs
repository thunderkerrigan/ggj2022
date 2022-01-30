using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;

// enum PhaseId {
//     PHASE_1,
//     PHASE_2
// }

public class Phase : MonoBehaviour
{
    [Tooltip("Duration of the timer for the phase. -1 = No timer")]
    [SerializeField] private int _maxTimer;

    [SerializeField] private bool _shouldSpawnEnemies;

    // [Tooltip("Max amount of enemies on the field. -1 = no limit")]
    // [SerializeField] private int maxEnemySpawnedCount;

    // [Tooltip("Amount of enemies spawned every 10 seconds")]
    // [SerializeField] private int enemySpawnRate;

    // TODO audio?

    [SerializeField] private bool _pvpEnabled;

    [Tooltip("Identifier of the scene, DO NOT CHANGE ONCE SET")]
    public string identifier;

    public bool ShouldSpawnEnemies() {
        return _shouldSpawnEnemies;
    }

    public int MaxTimer() {
        return _maxTimer;
    }

    public bool hasTimer() {
        return _maxTimer > 0;
    }

    public bool hasPVPEnabled() {
        return _pvpEnabled;
    }
}