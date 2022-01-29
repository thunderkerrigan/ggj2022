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

    public void spawn()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            var index = PhotonNetwork.PlayerList.ToList().IndexOf(player);
            var spawnpoint = GetSpawnpoint(index);
            var doudou = PhotonNetwork.Instantiate(
                Path.Combine("TeddyBears", "Prefabs", "Bear_" + index),
                spawnpoint.position, spawnpoint.rotation);
            doudou.GetPhotonView().TransferOwnership(player);
        }
    }
}