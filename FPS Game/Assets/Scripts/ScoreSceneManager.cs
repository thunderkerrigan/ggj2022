using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScoreSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI testMesh;

    void Awake()
    {
        var text = "Score\n\n";

        var i = 1 ;
        foreach( KeyValuePair<string,float> entry in ScoreSingleton.Instance.playerTimes.OrderBy( x => x.Value ).ThenByDescending( x => x.Key ) )
        {
            text += $"#{i} {entry.Key} — {entry.Value}sec\n";
            i += 1;
        }

        testMesh.text = text;
        
        StartCoroutine(nameof(RestartGameRoutine));

    }
    
    IEnumerator RestartGameRoutine()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom(false);
        PhotonNetwork.LoadLevel(0);
    }
}