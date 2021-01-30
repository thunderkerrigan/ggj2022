using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class DoudouManager : MonoBehaviourPunCallbacks
{
    public static DoudouManager Instance;

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

    public void spawnDoudous()
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

    public void onPlayerLootDoudou(Player player, GameObject doudou)
    {
        Hashtable hash = new Hashtable();
        hash.Add("PLAYER_LOOT_DOUDOU", doudou.name);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        PhotonNetwork.Destroy(doudou);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("PLAYER_LOOT_DOUDOU")) return;
        var doudouName = (string) changedProps["PLAYER_LOOT_DOUDOU"];
        var playerUserId = targetPlayer.NickName;
        ScoreSingleton.Instance.onPlayerLootItem(doudouName, playerUserId);
        CanvasManager.Instance.addCombatLog($"{playerUserId} find his Doudou");
    }
}