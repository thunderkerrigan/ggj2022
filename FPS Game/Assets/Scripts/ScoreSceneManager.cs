using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI testMesh;
    [SerializeField] GameObject restartButton;

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        print("IsMasterClient" + PhotonNetwork.IsMasterClient);

        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
        restartButton.SetActive(PhotonNetwork.IsMasterClient);
        var text = "Score\n\n";

        var i = 1 ;
        foreach( KeyValuePair<string,float> entry in ScoreSingleton.Instance.playerTimes.OrderBy( x => x.Value ).ThenByDescending( x => x.Key ) )
        {
            text += $"#{i} {entry.Key} — {entry.Value}sec\n";
            i += 1;
        }

        testMesh.text = text;
    }
    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        restartButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void restartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }
    
}