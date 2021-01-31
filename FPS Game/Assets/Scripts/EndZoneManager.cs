using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class EndZoneManager : MonoBehaviourPunCallbacks
{
    private DateTime _startTime;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = DateTime.Now;
    }
    
    public void onPlayerDetected(Player player)
    {
        var scoreSingleton = ScoreSingleton.Instance;

        if (!scoreSingleton.playerHaveDoudou(player.NickName))
        {
            // NO DOUDOU
            print("PLAYER DONT HAVE DOUDOU");
            return;
        }

        if (scoreSingleton.playerHaveFinish(player.NickName))
        {
            print("PLAYER ALREADY FINISH");
            // ALREADY FINISH
            return;
        }
        var difference = DateTime.Now.Subtract(_startTime); // could also write `now - otherTime`
        var time = Convert.ToSingle(difference.TotalSeconds);
        var hash = new Hashtable
        {
            {"PLAYER_FINISHED", time},
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("PLAYER_FINISHED")) return;
        print("PLAYER_FINISHED " + changedProps["PLAYER_FINISHED"]);
        var time = (float) changedProps["PLAYER_FINISHED"];
        CanvasManager.Instance.addCombatLog($"{targetPlayer.NickName} has finish");
        onPlayerEnterEndZone(targetPlayer.NickName, time);
    }

    private void onPlayerEnterEndZone(string playerNickName, float time)
    {
        var scoreSingleton = ScoreSingleton.Instance;
        print($"Player enterd: {playerNickName}");
        print($"TIME = {time}");
        scoreSingleton.onPlayerFinish(playerNickName, time);

        var players = PhotonNetwork.PlayerList;
        print("number of players " + players.Length);
        var everyOneIsArrived = players.All(playerInList => scoreSingleton.playerHaveFinish(playerInList.NickName));

        if (everyOneIsArrived)
        {
            PhotonNetwork.LoadLevel(2);
        }
    }
}