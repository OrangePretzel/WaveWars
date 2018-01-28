using UnityEngine;

public class Spawner : MonoBehaviour
{
	public int TEAM_ID;
	public float spawnDelay = 5;
	public GameObject thingToSpawn;
	public GameObject minions;

	public float lastSpawn;

	void FixedUpdate()
	{
		if (GameManager.IsGamePaused)
			return;

		var time = Time.time;
		var diff = time - lastSpawn;
		if (diff > spawnDelay)
		{
			Spawn();
			lastSpawn = time;
		}
	}

	void Spawn()
	{
		// spawn anything
		GameObject newMin = Instantiate(thingToSpawn, this.transform.position, Quaternion.identity, minions.transform);
		var MinionEntity = newMin.GetComponent<Entity>();
		var MinionType = newMin.GetComponent<Minion>();
		GameManager.AddEntity(MinionEntity); // make sure GameManager is aware of new minion
		PushNewlySpawned(MinionEntity, MinionType); // push it a lil so they don't all overlap
	}

	void PushNewlySpawned(Entity me, Minion mt)
	{
		Vector2 forceA = new Vector2(1, 0);
		Vector2 forceB = new Vector2(-1, 0);
		if (TEAM_ID == 0)
		{
			mt.GetComponent<Rigidbody2D>().AddForce(forceA);
		}
		else
		{
			mt.GetComponent<Rigidbody2D>().AddForce(forceB);
		}
	}
}
