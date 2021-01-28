using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("GAME IS FINISH");
        print("CURRENT SCORE of players");
        foreach (KeyValuePair<string, float> keyValuePair in ScoreSingleton.Instance.playerTimes)
        {
            print($"PLAYER:{keyValuePair.Key} : {keyValuePair.Value}");
        }
    }
    
}
