using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum WeaponType {
    Rateau
} 

public class WeaponTypePrefabProvider {

	public static string prefabPath(WeaponType type) {
		switch (type) {
			case WeaponType.Rateau:
				return "Prefabs/Drop_Rateau_TEST";
		}

		Debug.LogError("NO PREFAB PATH DEFINED FOR " + type.ToString());
		return "";
	}
}

/// Weapon found on the ground
public class WeaponDropItem: MonoBehaviourPunCallbacks
{

	public string audioType;
	public int audioClipIndex = -1;
	//public abstract override float Use();
	public WeaponType type = WeaponType.Rateau;

	private Spawnpoint spawnpoint;

	public void setSpawnpoint(Spawnpoint spawnpoint) {
		this.spawnpoint = spawnpoint;
		spawnpoint.setBusy(true);
	}

	private void OnDestroy() {
		spawnpoint.setBusy(false);
	}

	private void OnTriggerEnter(Collider other) {
		if (!photonView.IsMine)
		{
			return;
		}
		Debug.Log("RAKE TRIGGER ENTER " + other.gameObject.name);
		if (other.gameObject.tag == "Player") {
			// TODO: the player cannot pickup if he already has a weapon
			// TODO: Give weapon to player
			GameObject.Destroy(this.gameObject);
		}
	}
}
