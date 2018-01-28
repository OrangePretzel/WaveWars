using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct GameSettings
{
	public bool UnlimitedTransmission;
}

public class GameManager : MonoBehaviour
{
	// This is the single instance of the game manager
	public static GameManager Instance { get; private set; }

	// If true we are paused
	public bool IsPaused { get; private set; }
	public static bool IsGamePaused => Instance.IsPaused;

	[SerializeField]
	private GameSettings gameSettings = new GameSettings();
	public static GameSettings GameSettings { get { return Instance.gameSettings; } }

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
		if (IsInPlayerSelection)
		{
			int playerCount = 0;
			for (int i = 0; i < 4; i++)
			{
				var input = InputManager.GetPlayerInput(i);
				if (input.PlayerDevice != null)
					playerCount++;
				HandleSinglePlayerInputForPlayerSelectionScreen(i, input);
			}
			PlayerCount = playerCount;

			if (PlayerSelectScreen.AnyPlayerConnected() && PlayerSelectScreen.AreAllPlayersReady())
			{
				Debug.Log("Ready!");

				// TODO: Prep game

				UnpauseGame();

				HidePlayerSelectScreen();
			}
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				TogglePause();
			}

			if (PlayerCount == 0)
			{
				Debug.Log("No Players Connected!");

				ShowPlayerSelectScreen();

				PauseGame();
			}
		}
	}

	private void HandleSinglePlayerInputForPlayerSelectionScreen(int i, PlayerInput input)
	{
		if (input.PlayerDevice == null)
		{
			PlayerSelectScreen.UnAssignPlayerFromTeams(i);
			PlayerSelectScreen.SetPlayerEnabled(i, false);
		}
		else
		{
			PlayerSelectScreen.SetPlayerEnabled(i, true);
			if (input.Select)
			{
				PlayerSelectScreen.MakePlayerReady(i, true);
			}

			if (input.Back)
			{
				if (PlayerSelectScreen.IsPlayerReady(i))
				{
					PlayerSelectScreen.MakePlayerReady(i, false);
				}
				else
				{
					PlayerSelectScreen.SetPlayerEnabled(i, false);
					InputManager.DisconnectPlayerDevice(i);
				}
			}

			if (input.HorizontalMovement != 0 && !PlayerSelectScreen.IsPlayerReady(i))
			{
				PlayerSelectScreen.AssignPlayerToTeam(i, ((int)Mathf.Sign(input.HorizontalMovement) + 1) / 2);
			}
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

	public static List<Entity> GetFriendlyEntities(int myTeamID)
	{
		switch (myTeamID)
		{
			case Entity.TEAM_A_ID:
				return Instance.TeamAEntities;
			case Entity.TEAM_B_ID:
				return Instance.TeamBEntities;
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

	public static void AddEntity(Entity entity)
	{
		if (entity.TeamID == Entity.TEAM_A_ID)
		{
			Instance.TeamAEntities.Add(entity);
		}
		else if (entity.TeamID == Entity.TEAM_B_ID)
		{
			Instance.TeamBEntities.Add(entity);
		}
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

	public bool IsInPlayerSelection;
	public PlayerSelectScreen PlayerSelectScreen;

	private void ShowPlayerSelectScreen()
	{
		IsInPlayerSelection = true;

		PlayerSelectScreen.ToggleScreen(true);
	}

	private void HidePlayerSelectScreen()
	{
		IsInPlayerSelection = false;

		PlayerSelectScreen.ToggleScreen(false);
	}
}
