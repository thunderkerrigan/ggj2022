using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
	PhotonView PV;

	GameObject controller;

	void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

	void Start()
	{
		if(PV.IsMine)
		{
			CreateController();
			StartGameCountDown();
		}
	}

	void CreateController()
	{
		Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
		controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
	}

	public void Die()
	{
		PhotonNetwork.Destroy(controller);
		CreateController();
	}
	
	// Countdown Logic
	public int countDownValue = 5;

	private void StartGameCountDown()
	{
		StartCoroutine(nameof(LowerCountDownRoutine));
	}

	IEnumerator LowerCountDownRoutine()
	{
		while (true)
		{
			print($"Starting In {countDownValue}");
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
		print("Find your DOUDOU");
		controller.GetComponent<PlayerController>().canMove = true;
	}
	
}