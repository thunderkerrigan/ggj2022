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
    
    public int gameDuration = 60;
    // Start is called before the first frame update
    void Awake()
    {    
        Debug.Log("Awake player ");
        playerInputManager = GetComponent<PlayerInputManager>();
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
                StartCoroutine(Cooldown());
            }
        }

        
    }

    private IEnumerator Cooldown()
    {
        // Propagate timer if something is listening to it
        if (OnTimerUpdate != null)
        {
            OnTimerUpdate(gameDuration);
        }
        yield return new WaitForSeconds(1);
        gameDuration--;
        if (gameDuration > 0)
        {
            StartCoroutine(Cooldown());
        }
        else
        {
            Debug.Log("Game Over");
            GoToScore();
        }
    }
    
    private void GoToScore()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        PhotonNetwork.LoadLevel(3);
    }

}
