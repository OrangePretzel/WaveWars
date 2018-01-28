using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
	//public Player spawnMyPlayer;
	public const int SECONDS_TIL_SPAWN = 10;
	public bool spawnQueued;

	private Player pts;
	public float DelayTilSpawn;

	void Awake()
	{
		spawnQueued = false;
	}

	private void FixedUpdate()
	{
		if (!spawnQueued || GameManager.IsGamePaused)
			return;

		DelayTilSpawn -= Time.deltaTime;
		if (DelayTilSpawn <= 0)
		{
			pts.gameObject.SetActive(true);
			pts.transform.position = transform.position;
			spawnQueued = false;
			pts = null;
		}
	}

	public void Spawn(Player spawnMe, float delay = SECONDS_TIL_SPAWN)
	{
		if (!spawnQueued)
		{
			DelayTilSpawn = delay;
			pts = spawnMe;
			spawnQueued = true;
		}
	}

	public bool IsSpawning()
	{
		return spawnQueued;
	}
}
