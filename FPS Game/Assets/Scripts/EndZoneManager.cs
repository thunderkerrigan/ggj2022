using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class EndZoneManager : MonoBehaviourPunCallbacks
{
    Dictionary<string, float> _playerTimes = new Dictionary<string, float>();
    private float _startTime;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
    }
    
    public void onPlayerDetected(Player player)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        

        var playerUserId = player.UserId;
        if (!_playerTimes.ContainsKey(playerUserId))
        {
            print($"Player enterd: {playerUserId}");
            var time = _startTime - DateTimeOffset.Now.ToUnixTimeSeconds();
            print($"TIME = {time}");
            _playerTimes.Add(playerUserId, time);

            var everyOneIsArrived = true;
            Player[] players = PhotonNetwork.PlayerList;
            print("number of players " + players.Length);
            
            foreach (var playerInList in players)
            {
                print("NickName" + playerInList.NickName);
                if (playerInList.NickName != null && !_playerTimes.ContainsKey(playerInList.NickName))
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
                    print($"PLAYER:{keyValuePair.Key} : {keyValuePair.Value}");
                }
            }
        }
        else
        {
            print($"Player already enterd: {playerUserId}");
        }
    }
}