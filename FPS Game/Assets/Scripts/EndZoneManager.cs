using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EndZoneManager : MonoBehaviourPunCallbacks
{
    private DateTime _startTime;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = DateTime.Now;
        print(_startTime);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        
    }

    public void onPlayerDetected(Player player)
    {
        var playerUserId = player.NickName;

        Hashtable hash = new Hashtable
        {
            {"PLAYER_FINISHED", playerUserId},
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    private void onPlayerLogic(string playerUserId)
    {
        var scoreSingleton = ScoreSingleton.Instance;
        
        if (!scoreSingleton.playerTimes.ContainsKey(playerUserId))
        {
            var time = (DateTime.Now - _startTime).Seconds;

            print($"Player enterd: {playerUserId}");
            print($"TIME = {time}");
            scoreSingleton.playerTimes.Add(playerUserId, time);

            var everyOneIsArrived = true;
            Player[] players = PhotonNetwork.PlayerList;
            print("number of players " + players.Length);

            foreach (var playerInList in players)
            {
                print("NickName" + playerInList.NickName);
                if (!scoreSingleton.playerTimes.ContainsKey(playerInList.NickName))
                {
                    everyOneIsArrived = false;
                }
            }

            if (everyOneIsArrived)
            {
                PhotonNetwork.LoadLevel(2);
            }
        }
        else
        {
            print($"Player already enterd: {playerUserId}");
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        print(propertiesThatChanged);
        var playerId = (string) propertiesThatChanged["PLAYER_FINISHED"];
        print(playerId);
        onPlayerLogic(playerId);
    }
}