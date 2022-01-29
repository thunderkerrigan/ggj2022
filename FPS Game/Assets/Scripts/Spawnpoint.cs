using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
	[SerializeField] GameObject graphics;

	private bool isBusy = false;

	public void setBusy(bool busy) {
		this.isBusy = busy;
	}

	public bool canSpawn() {
		return (this.isBusy == false);
	}

	void Awake()
	{
		graphics.SetActive(false);
	}
}
