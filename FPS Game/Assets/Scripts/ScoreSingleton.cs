using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class ScoreSingleton : MonoBehaviour
{
    public static ScoreSingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public Dictionary<string, float> playerTimes = new Dictionary<string, float>();

    private Dictionary<string, List<string>> playerObjects = new Dictionary<string, List<string>>();

    public string scoreText = "Victory";

    public bool playerHaveFinish(string playerName)
    {
        return playerTimes.ContainsKey(playerName) && playerHaveDoudou(playerName);
    }

    public void onPlayerFinish(string playerName, float time)
    {
        playerTimes.Add(playerName, time);
    }
    
    public bool playerHaveDoudou(string playerName)
    {
        return playerObjects.ContainsKey(playerName);
    }

    public void onPlayerLootItem(string itemName, string playerName)
    {
        if (playerObjects.ContainsKey(playerName))
        {
            playerObjects[playerName].Add(itemName);
        }
        else
        {
            playerObjects.Add(playerName, new List<string> {itemName});
        }
    }
    
}