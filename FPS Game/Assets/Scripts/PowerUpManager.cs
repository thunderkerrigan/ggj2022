using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    List<Spawnpoint> _spawnpoints;

    void Awake()
    {
        Instance = this;
        _spawnpoints = GetComponentsInChildren<Spawnpoint>().OrderBy(x => Random.value).ToList();
    }

    [CanBeNull]
    private Transform GetSpawnpoint(int index)
    {
        if (index >= 0 && _spawnpoints.Count > index) return _spawnpoints[index].transform;
        return null;
    }

    public void spawnPowerUps()
    {
        for (var i = 0; i < PhotonNetwork.PlayerList.Length * 2; i++)
        {
            var spawnpoint = GetSpawnpoint(i);
            if (spawnpoint != null)
            {
                var powerUpType = (PowerUpType) Random.Range(0, Enum.GetValues(typeof(PowerUpType)).Length);
                switch (powerUpType)
                {
                    case PowerUpType.MachineGun:
                        PhotonNetwork.Instantiate(
                            Path.Combine("PhotonPrefabs", "PowerUpMachineGun"),
                            spawnpoint.position, spawnpoint.rotation);
                        break;
                    case PowerUpType.Speed:
                        PhotonNetwork.Instantiate(
                            Path.Combine("PhotonPrefabs", "PowerUpSpeed"),
                            spawnpoint.position, spawnpoint.rotation);
                        break;
                    case PowerUpType.ReverseControl:
                        PhotonNetwork.Instantiate(
                            Path.Combine("PhotonPrefabs", "PowerUpReverseControl"),
                            spawnpoint.position, spawnpoint.rotation);
                        break;
                    case PowerUpType.Stunt:
                        PhotonNetwork.Instantiate(
                            Path.Combine("PhotonPrefabs", "PowerUpStunt"),
                            spawnpoint.position, spawnpoint.rotation);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}