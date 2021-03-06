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

public class WeaponDropManager : MonoBehaviourPunCallbacks
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
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnWeapon());
        }
    }

    public void stopSpawn()
    {
        StopCoroutine(SpawnWeapon());
    }


    // TODO: we want a max weapon spawned

    IEnumerator SpawnWeapon()
    {
        yield return new WaitForSeconds(this.spawnCooldown);

        if (this.enabled == false) { StopCoroutine(SpawnWeapon()); yield break; }

        // SPAWN LOGIC


        var shuffledSpawnpoints = Shuffle(spawnpoints: this.spawnpoints);

        Spawnpoint spawnpoint = null;
        foreach (Spawnpoint sp in shuffledSpawnpoints) {
            if (sp.canSpawn() == false) { continue; }
            
            spawnpoint = sp;
        }

        WeaponType weaponType = (WeaponType) weaponTypes.GetValue(UnityEngine.Random.Range(0, weaponTypes.Length-1));
        var prefabPath = WeaponTypePrefabProvider.prefabPath(weaponType);
        var weaponGameObject = (GameObject) PhotonNetwork.Instantiate(Path.Combine(prefabPath), spawnpoint.transform.position, spawnpoint.transform.rotation);
        var weapon = weaponGameObject.GetComponentInChildren<WeaponDropItem>();
        weapon.setSpawnpoint(spawnpoint: spawnpoint);
        Debug.Log("SPAWN");

        StartCoroutine(SpawnWeapon());
    }

    private List<Spawnpoint> Shuffle(List<Spawnpoint> spawnpoints) {
        var shuffledSpawnpoints = spawnpoints;
        for (int i = 0; i < shuffledSpawnpoints.Count - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, shuffledSpawnpoints.Count);
            var tempGO = shuffledSpawnpoints[rnd];
            shuffledSpawnpoints[rnd] = shuffledSpawnpoints[i];
            shuffledSpawnpoints[i] = tempGO;
        }

        return shuffledSpawnpoints;
 }
}