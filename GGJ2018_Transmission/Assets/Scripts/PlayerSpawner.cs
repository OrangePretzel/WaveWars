using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
	//public Player spawnMyPlayer;
	public const int SECONDS_TIL_SPAWN = 10;
	public bool spawnQueued;
	void Awake()
	{
		spawnQueued = false;
	}

	public void Spawn(Player spawnMe, float delay = SECONDS_TIL_SPAWN)
	{
		if (!spawnQueued)
		{
			Invoke("ExecuteSpawn", delay);
			spawnQueued = true;
		}
	}

	public bool IsSpawning()
	{
		return spawnQueued;
	}

	public void ExecuteSpawn(Player spawnMe)
	{
		//if (playerID =)
		spawnMe.gameObject.SetActive(true);
		spawnQueued = false;
	}
}
