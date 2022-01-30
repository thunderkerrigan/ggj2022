using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;


public delegate void TimerUpdateHandler(float value);
public class LocalGameManager : MonoBehaviour
{
    public PhotonView PV;
    public event TimerUpdateHandler OnTimerUpdate;
    private PlayerInputManager playerInputManager;
    private PhaseManager phaseManager;
    
    public int gameDuration = 60;
    // Start is called before the first frame update
    void Awake()
    {    
        Debug.Log("Awake player ");
        playerInputManager = GetComponent<PlayerInputManager>();
        phaseManager = GetComponent<PhaseManager>();
        Debug.Log(PhotonNetwork.PlayerList.ToList().Count);
        if (PhotonNetwork.OfflineMode)
        {
            Debug.Log("Offline mode; 2 players!");
            playerInputManager.JoinPlayer();
            playerInputManager.JoinPlayer();
            StartCoroutine(Cooldown());

        }
        else if (PV.IsMine)
        {
            
            var index = PhotonNetwork.PlayerList.ToList().IndexOf(PhotonNetwork.LocalPlayer);
            Debug.Log("spawning player " + index);
            var spawnPoint = SpawnManager.Instance.GetSpawnpoint(index);
            Debug.Log("spawning player at " + spawnPoint.position);
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"),  spawnPoint.position,
                spawnPoint.rotation, 0, new object[] {PV.ViewID, index});
            if (PhotonNetwork.IsMasterClient)
            {
                phaseManager.StartGame();
                phaseManager.OnDefeatHandler += GoToScore;
                StartCoroutine(Cooldown());
            }
        }

        
    }

    private IEnumerator Cooldown()
    {
        // Propagate timer if something is listening to it
        while (gameDuration > 0)
        {
            if (OnTimerUpdate != null)
            {
                OnTimerUpdate(gameDuration);
            }
            yield return new WaitForSeconds(1);
            gameDuration--;

            StartCoroutine(Cooldown());
        }
    }

    private void GoToScore(string reason)
    {
        ScoreSingleton.Instance.scoreText = reason;
        if (!PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        PhotonNetwork.LoadLevel(2);
    }

}
