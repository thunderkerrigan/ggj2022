using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Linq;
using DG.Tweening;
using Doozy.Engine.Soundy;
using ExitGames.Client.Photon;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    GameObject endZoneManager;
    private bool gameHasStarted = false;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            // var gameHasStarted = false;
            // if (PhotonNetwork.LocalPlayer.CustomProperties.Keys.Contains("GameHasStarted"))
            // {
            //     gameHasStarted = (bool)PhotonNetwork.LocalPlayer.CustomProperties["GameHasStarted"];
            // }
            // else
            // {
            //     var hash = new Hashtable();
            //     hash.Add("GameHasStarted", false);
            //     PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            // }
            CreateController();
            if (!gameHasStarted)
            {
                StartGameCountDown();
                controller.GetComponent<PlayerController>().canMove = false;
            }
            

            if (PhotonNetwork.IsMasterClient)
            {
                print("Create Doudous");
                DoudouManager.Instance.spawnDoudous();
            }
        }
    }

    void CreateController()
    {
        var index = PhotonNetwork.PlayerList.ToList().IndexOf(PhotonNetwork.LocalPlayer);
        FindObjectOfType<ScoreCanvasManager>().gameObject.GetComponent<TextMeshProUGUI>().text = $"PLAYER #{index}";
        var spawnpoint = SpawnManager.Instance.GetSpawnpoint(index);
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"),
            spawnpoint.position,
            spawnpoint.rotation, 0, new object[] {PV.ViewID, index});
    }


    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    // Countdown Logic
    public int countDownValue = 3;

    private void StartGameCountDown()
    {
        StartCoroutine(nameof(LowerCountDownRoutine));
    }

    IEnumerator LowerCountDownRoutine()
    {
        while (true)
        {
            print($"Starting In {countDownValue}");
            FindObjectOfType<ScoreCanvasManager>().gameObject.GetComponent<TextMeshProUGUI>().text =
                $"Starting In {countDownValue}";
            countDownValue -= 1;
            if (countDownValue < 0)
            {
                onCountDownFinish();
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void onCountDownFinish()
    {
        FindObjectOfType<ScoreCanvasManager>().gameObject.GetComponent<TextMeshProUGUI>().text = $"Find doudou and go endzone";
        controller.GetComponent<PlayerController>().canMove = true;
        SoundyManager.Play("General", "main");
        var hash = new Hashtable();
        hash.Add("GameHasStarted", true);
        gameHasStarted = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
}