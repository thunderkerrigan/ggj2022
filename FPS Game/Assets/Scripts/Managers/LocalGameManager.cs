using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public delegate void TimerUpdateHandler(float value);
public class LocalGameManager : MonoBehaviour
{
    
    public event TimerUpdateHandler OnTimerUpdate;
    
    public int gameDuration = 60;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Cooldown());
    }

    // Update is called once per frame
    void Update()
    {
        
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
            RestartGame();
        }
    }
    
    private void RestartGame()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
        PhotonNetwork.LoadLevel(0);
    }

}
