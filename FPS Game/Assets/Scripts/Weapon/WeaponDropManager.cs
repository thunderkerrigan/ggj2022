using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using System;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WeaponDropManager: MonoBehaviourPunCallbacks
{
    public static WeaponDropManager Instance;

    private List<Spawnpoint> spawnpoints;

    [SerializeField] private float spawnCooldown;

    private Array weaponTypes = Enum.GetValues(typeof(WeaponType));
    
    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>().OrderBy(x => UnityEngine.Random.value).ToList();
    }

    private Transform GetSpawnpoint(int index)
    {
        return spawnpoints[index].transform;
    }

     public void startSpawn()
    {
        this.enabled = true;
        StartCoroutine(SpawnWeapon());
    }

    public void stopSpawn() {
        StopCoroutine(SpawnWeapon());
        this.enabled = false;
    }     


    IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(this.spawnCooldown);

        if (this.enabled == false) { StopCoroutine(SpawnWeapon()); yield break; }

        // SPAWN LOGIC
        var spawnpoint = spawnpoints[UnityEngine.Random.Range(0, spawnpoints.Count-1)];
        
        // TODO: Photon
        // TODO: we want to spawn something only on the spawners that are not "busy"
        //var enemy = Instantiate(Resources.Load("Prefabs/Enemy_Base"), spawnpoint.transform.position, spawnpoint.transform.rotation);
       // var enemy = PhotonNetwork.Instantiate("Prefabs/Enemy_Base", spawnpoint.transform.position, spawnpoint.transform.rotation);
        Debug.Log("SPAWN");

        StartCoroutine(SpawnWeapon());
    }
}