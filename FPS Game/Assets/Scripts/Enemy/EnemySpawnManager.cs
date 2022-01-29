using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class EnemySpawnManager: MonoBehaviourPunCallbacks
{
    public static EnemySpawnManager Instance;

    public float spawnCooldown;

    private List<Spawnpoint> spawnpoints;

    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>().OrderBy(x => Random.value).ToList();
    }

    private Transform GetSpawnpoint(int index)
    {
        return spawnpoints[index].transform;
    }

    public void startSpawn()
    {
        this.enabled = true;
        StartCoroutine(SpawnEnemy());
    }

    public void stopSpawn() {
        StopCoroutine(SpawnEnemy());
        this.enabled = false;
    }     

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(this.spawnCooldown);

        if (this.enabled == false) { StopCoroutine(SpawnEnemy()); yield break; }

        // SPAWN LOGIC
        var spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count-1)];
        
        var enemy = Instantiate(Resources.Load("Prefabs/Enemy_Base"), spawnpoint.transform.position, spawnpoint.transform.rotation);
       // var enemy = PhotonNetwork.Instantiate("Prefabs/Enemy_Base", spawnpoint.transform.position, spawnpoint.transform.rotation);
        Debug.Log("SPAWN");

        StartCoroutine(SpawnEnemy());
    }
}