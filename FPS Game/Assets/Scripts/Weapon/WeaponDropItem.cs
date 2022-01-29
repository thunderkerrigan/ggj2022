using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum WeaponType {
    Rateau
} 

public class WeaponTypePrefabProvider {

	static string prefabPath(WeaponType type) {
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
    public PlayerController playerController;
	public string audioType;
	public int audioClipIndex = -1;
	//public abstract override float Use();
	public WeaponType type = WeaponType.Rateau;
}
