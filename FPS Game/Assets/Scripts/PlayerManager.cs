using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Linq;
using TMPro;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    GameObject controller;

    GameObject endZoneManager;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
            StartGameCountDown();
        }

        print($"PhotonNetwork.IsMasterClient {(PhotonNetwork.IsMasterClient)}");
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position,
            spawnpoint.rotation, 0, new object[] {PV.ViewID});
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
            FindObjectOfType<ScoreCanvasManager>().gameObject.GetComponent<TextMeshProUGUI>().text = $"Starting In {countDownValue}";
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
        FindObjectOfType<ScoreCanvasManager>().gameObject.GetComponent<TextMeshProUGUI>().text = $"GO to finish zone";
        controller.GetComponent<PlayerController>().canMove = true;
    }
}