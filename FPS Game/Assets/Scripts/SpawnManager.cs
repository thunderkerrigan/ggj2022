using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;

	Spawnpoint[] spawnpoints;
	private int usedSpawnPoint = 0;

	void Awake()
	{
		Instance = this;
		spawnpoints = GetComponentsInChildren<Spawnpoint>();
	}

	public Transform GetSpawnpoint()
	{
		usedSpawnPoint++;
		return spawnpoints[Random.Range(usedSpawnPoint - 1, spawnpoints.Length)].transform;
	}
}
