using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputManager : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	private const int MAX_PLAYERS = 4;
	private PlayerIndex?[] playerIndexMap = new PlayerIndex?[4];
	private GamePadState[] playerGamePadStates = new GamePadState[4];

	private void FixedUpdate()
	{
		UpdatePlayerIndexMap();
	}

	float x = 0;
	float dir = 1;
	private void Update()
	{
		UpdateGamePadStates();

		x += Time.deltaTime * 0.1f * dir;
		if (dir > 0 && x >= 1)
		{
			dir = -dir;
			x = 1;
		}
		else if (dir < 0 && x <= 0)
		{
			dir = -dir;
			x = 0;
		}

		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			SetVibration(i, x, x);
		}
	}

	private void OnGUI()
	{
		const bool DEBUG_GUI = true;
		if (DEBUG_GUI)
		{
			var p1 = playerIndexMap[0] == null ? "No Input Method" : playerIndexMap[0].ToString();
			var p2 = playerIndexMap[1] == null ? "No Input Method" : playerIndexMap[1].ToString();
			var p3 = playerIndexMap[2] == null ? "No Input Method" : playerIndexMap[2].ToString();
			var p4 = playerIndexMap[3] == null ? "No Input Method" : playerIndexMap[3].ToString();
			GUI.Label(new Rect(0, 0, 1000, 1000), $@"
Player 1: {p1}
Player 2: {p2}
Player 3: {p3}
Player 4: {p4}
");
		}
	}

	public void SetVibration(int playerID, float leftMotor, float rightMotor)
	{
		if (playerID >= MAX_PLAYERS || playerID < 0 || !playerIndexMap[playerID].HasValue)
			return; // Invalid player ID or no controller connected

		GamePad.SetVibration(playerIndexMap[playerID].Value, leftMotor, rightMotor);
	}

#endif
	private void UpdatePlayerIndexMap()
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		// TODO: Add support for plug and play
		for (int i = 0; i < MAX_PLAYERS; i++) // We only support maximum four players
		{
			// For each player try and get the state
			PlayerIndex playerIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(playerIndex);
			if (testState.IsConnected) // Check if connected
			{
				SetNextPlayerIndex(playerIndex); // Add them to the list of connected players
			}
		}
#endif
	}

	private void SetNextPlayerIndex(PlayerIndex playerIndex)
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		const int INVALID = -1; // Represents an invalid player/controller map
		int potentialPos = INVALID;

		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			if (playerIndexMap[i] == playerIndex)
			{
				return; // Already using this player index
			}
			else if (playerIndexMap[i] == null && potentialPos == INVALID)
			{
				// Potentially a place for this player index (still need to check the rest of the array)
				potentialPos = i;
			}
		}

		if (potentialPos != INVALID)
		{
			// Found a position for the controller
			playerIndexMap[potentialPos] = playerIndex;
		}
#endif
	}

	private void UpdateGamePadStates()
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		PlayerIndex? playerIndex;
		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			playerIndex = playerIndexMap[i]; // For each player, get the associated gamepad
			if (playerIndex == null)
				continue;

			// Update the gamepad state
			playerGamePadStates[i] = GamePad.GetState(playerIndex.Value);

			if (!playerGamePadStates[i].IsConnected)
			{
				playerIndexMap[i] = null;
			}
		}
#endif
	}
}
