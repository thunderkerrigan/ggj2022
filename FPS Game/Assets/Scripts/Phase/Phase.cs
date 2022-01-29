using System.Collections;
using System.IO;
using Photon.Pun;
using UnityEngine;


public class Phase : MonoBehaviour
{
    [Tooltip("Duration of the timer for the phase. -1 = No timer")]
    [SerializeField] private int maxTimer;

    // [Tooltip("Max amount of enemies on the field. -1 = no limit")]
    // [SerializeField] private int maxEnemySpawnedCount;

    // [Tooltip("Amount of enemies spawned every 10 seconds")]
    // [SerializeField] private int enemySpawnRate;

    // TODO audio?

    [SerializeField] private bool pvpEnabled;

    [Tooltip("Identifier of the scene, DO NOT CHANGE ONCE SET")]
    public string identifier;
}