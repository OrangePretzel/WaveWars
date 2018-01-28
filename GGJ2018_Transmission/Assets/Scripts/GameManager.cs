using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// This is the single instance of the game manager
	public static GameManager Instance { get; private set; }

	// If true we are paused
	public bool IsPaused { get; private set; }

	public static bool IsGamePaused => Instance.IsPaused;

	private int PlayerCount = 0;

	[SerializeField]
	private List<Entity> TeamAEntities = new List<Entity>();
	[SerializeField]
	private List<Entity> TeamBEntities = new List<Entity>();

	// Awake is fired BEFORE Start
	void Awake()
	{
		// No instance yet
		if (Instance == null)
		{
			// This is the instance
			Instance = this;
		}
		// There is an instance already
		else if (Instance != this)
		{
			// Destroy this
			Destroy(this);
			return;
		}

		BindInputManagerEvents();

		FindAllEntitiesInScene();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause();
		}
	}

	public static List<Entity> GetEnemyEntities(int myTeamID)
	{
		switch (myTeamID)
		{
			case Entity.TEAM_A_ID:
				return Instance.TeamBEntities;
			case Entity.TEAM_B_ID:
				return Instance.TeamAEntities;
			default:
				return new List<Entity>();
		}
	}

	private void BindInputManagerEvents()
	{
		InputManager.OnResetPlayerDevices += ejectedPlayerCounts =>
		{
			PauseGame();
		};
	}

	private void FindAllEntitiesInScene()
	{
		var teamAEntities = GameObject.FindGameObjectsWithTag("TeamAEntity").Select(go => go.GetComponent<Entity>());
		var teamBEntities = GameObject.FindGameObjectsWithTag("TeamBEntity").Select(go => go.GetComponent<Entity>());

		TeamAEntities.AddRange(teamAEntities);
		TeamBEntities.AddRange(teamBEntities);
	}

	public static void RemoveEntity(Entity entity)
	{
		Instance.TeamAEntities.Remove(entity);
		Instance.TeamBEntities.Remove(entity);
	}

	#region Pause/Play

	// Toggle the pause
	public void TogglePause()
	{
		// Self explanatory?
		if (IsPaused)
			UnpauseGame();
		else
			PauseGame();
	}

	// Time.timescale controls the update rate of the game. 0 is paused, 1 is normal, in between is slow motion and greater than 1 is fast
	// Pause the game
	public void PauseGame()
	{
		// Pause the game
		Time.timeScale = 0;
		IsPaused = true;
	}

	// Unpause the game
	public void UnpauseGame()
	{
		// Unpause the game
		Time.timeScale = 1;
		IsPaused = false;
	}

	#endregion

}
