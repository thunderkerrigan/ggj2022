using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class EndZoneManager : MonoBehaviourPunCallbacks
{
    Dictionary<string, float> _playerTimes = new Dictionary<string, float>();
    private Player[] _playerList;
    private float _startTime;

    // Start is called before the first frame update
    void Start()
    {
        _playerList = PhotonNetwork.PlayerList;
        _startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void onPlayerDetected(Player player)
    {
        
        var playerUserId = player.UserId;
        if (!_playerTimes.ContainsKey(playerUserId))
        {
            print($"Player enterd: {playerUserId}");
            var time = _startTime - DateTimeOffset.Now.ToUnixTimeSeconds();
            print($"TIME = {time}");
            _playerTimes.Add(playerUserId, time);

            var everyOneIsArrived = true;
            print("number of players " + _playerList.Length);
            
            foreach (var playerInList in _playerList)
            {
                print("UserId" + playerInList.UserId);
                if (playerInList.UserId != null && !_playerTimes.ContainsKey(playerInList.UserId))
                {
                    everyOneIsArrived = false;
                }
            }

            if (everyOneIsArrived)
            {
                print("GAME IS FINISH");
                print("CURRENT SCORE of players");
                foreach (KeyValuePair<string,float> keyValuePair in _playerTimes)
                {
                    print("PLAYER:" + keyValuePair.Key + $"{keyValuePair.Value}");
                }
            }
        }
        else
        {
            print($"Player already enterd: {playerUserId}");
        }
    }
}