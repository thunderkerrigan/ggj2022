using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class DoudouManager : MonoBehaviourPunCallbacks
{
    public static DoudouManager Instance;

    Spawnpoint[] spawnpoints;

    void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }

    private Transform GetSpawnpoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform;
    }

    public void spawnDoudous()
    {
        var colors = new List<Color> {Color.magenta, Color.blue, Color.yellow, Color.green};
        var colorIndex = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            var spawnpoint = GetSpawnpoint();
            var doudou = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Doudou"),
                spawnpoint.transform.position, spawnpoint.transform.rotation);
            doudou.GetPhotonView().TransferOwnership(player);
            doudou.gameObject.GetComponent<MeshRenderer>().material.color = colors[colorIndex];
            colorIndex++;
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
        FindObjectOfType<ScoreCanvasManager>().gameObject.GetComponent<TextMeshProUGUI>().text =
            $"{targetPlayer} loot doudou";
    }
}