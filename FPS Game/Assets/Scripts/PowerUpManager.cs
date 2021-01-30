using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    List<Spawnpoint> _spawnpoints;

    void Awake()
    {
        Instance = this;
        _spawnpoints = GetComponentsInChildren<Spawnpoint>().OrderBy(x => Random.value).ToList();
    }

    private Transform GetSpawnpoint(int index)
    {
        return _spawnpoints[index].transform;
    }

    public void spawnPowerUps()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var spawnpoint = GetSpawnpoint(i);
            var powerUp = PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "PowerUpMachineGun"),
                spawnpoint.position, spawnpoint.rotation);
        }
    }
}
