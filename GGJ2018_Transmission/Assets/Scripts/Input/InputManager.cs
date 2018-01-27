using System;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public struct PlayerInput
{
	public int PlayerID;
	public PlayerInputMethod InputMethod;

	public float HorizontalMovement;
	public float VerticalMovement;
	public float HorizontalAim;
	public float VerticalAim;

	public bool PushTransmission;
	public bool PullTransmission;

	public bool Menu;
	public bool Select;

	public Vector3 FixAimForKeyboard(Vector3 originPos)
	{
		// TODO: Test this
		if (InputMethod == PlayerInputMethod.Keyboard)
		{
			return (Camera.main.ScreenToWorldPoint(new Vector3(HorizontalAim, VerticalAim)) - originPos).normalized;
		}
		return new Vector3(HorizontalAim, VerticalAim);
	}

	public override string ToString()
	{
		return $@"Player: ({PlayerID})
Input Method: ({InputMethod})
Movement: ({HorizontalMovement}, {VerticalMovement})
Aim: ({HorizontalAim}, {VerticalAim})
Push: ({PushTransmission})
Pull: ({PullTransmission})
Menu: ({Menu})
Select: ({Select})";
	}
}

public enum PlayerInputMethod
{
	None,
	Keyboard,
	Controller1,
	Controller2,
	Controller3,
	Controller4
}

public class InputManager : MonoBehaviour
{
	private const int MAX_PLAYERS = 4;

	// TODO: Probably should map from PlayerInputMethod to Player ID instead of this (vice versa)
	private PlayerInputMethod[] playerInputMethods = new PlayerInputMethod[MAX_PLAYERS];
	private PlayerInput[] playerInputs = new PlayerInput[MAX_PLAYERS];

	private void FixedUpdate()
	{
		ScanForKeyboard();
		ScanForControllers();
		DisconnectInvalidInputMethods();
	}

	private void Update()
	{
		UpdatePlayerInput();
	}

	private void OnGUI()
	{
		const bool DEBUG_GUI = false;
		if (DEBUG_GUI)
		{
			GUI.Label(new Rect(0, 0, 1000, 1000), $@"
Player 1: {playerInputMethods[0]}
Player 2: {playerInputMethods[1]}
Player 3: {playerInputMethods[2]}
Player 4: {playerInputMethods[3]}
");
		}
	}

	public void SetNextPlayer(PlayerInputMethod playerInputMethod)
	{
		if (playerInputMethod == PlayerInputMethod.None)
			return; // What are you doing?

#if !UNITY_STANDALONE_WIN && !UNITY_EDITOR_WIN
		// No controller support on non Windows platforms :(
		if (playerInputMethod != PlayerInputMethod.Keyboard)
		{
			Debug.LogWarning("Sorry no controller support on this platform");
			return;
		}
#endif

		const int INVALID = -1; // Represents an invalid player/controller map
		int potentialPos = INVALID;

		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			if (playerInputMethods[i] == playerInputMethod)
			{
				return; // Already using this player index
			}
			else if (playerInputMethods[i] == PlayerInputMethod.None && potentialPos == INVALID)
			{
				// Potentially a place for this player index (still need to check the rest of the array)
				potentialPos = i;
			}
		}

		if (potentialPos != INVALID)
		{
			// Found a position for the controller
			Debug.Log($"Player {potentialPos + 1} is using {playerInputMethod}");
			playerInputMethods[potentialPos] = playerInputMethod;
			playerInputs[potentialPos].InputMethod = playerInputMethod;
		}
	}

	public bool GetPlayerInput(int playerID, ref PlayerInput playerInput)
	{
		if (playerID < 0 || playerID >= MAX_PLAYERS || playerInputMethods[playerID] == PlayerInputMethod.None)
			return false;

		playerInput = playerInputs[playerID];

		return true;
	}

	private void UpdatePlayerInput()
	{
		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			var playerInputMethod = playerInputMethods[i];
			playerInputs[i].InputMethod = playerInputMethod;
			playerInputs[i].PlayerID = i + 1;
			switch (playerInputMethod)
			{
				case PlayerInputMethod.Keyboard:
					UpdateKeyboardInput(ref playerInputs[i]);
					break;
				case PlayerInputMethod.Controller1:
				case PlayerInputMethod.Controller2:
				case PlayerInputMethod.Controller3:
				case PlayerInputMethod.Controller4:
					UpdateControllerInput(ref playerInputs[i], (int)playerInputMethod - 2); // 2 is the offset for controllers
					break;
				case PlayerInputMethod.None:
				default:
					continue;
			}
		}
	}

	#region Keyboard Support

	private void UpdateKeyboardInput(ref PlayerInput playerInput)
	{
		playerInput.HorizontalMovement = Input.GetAxis("Keyboard_MovementHorizontal");
		playerInput.VerticalMovement = Input.GetAxis("Keyboard_MovementVertical");
		playerInput.HorizontalAim = Input.mousePosition.x;
		playerInput.VerticalAim = Input.mousePosition.y;

		playerInput.PushTransmission = Input.GetButton("Keyboard_PushTransmission");
		playerInput.PullTransmission = Input.GetButton("Keyboard_PullTransmission");

		playerInput.Menu = Input.GetButton("Keyboard_Menu");
		playerInput.Select = Input.GetButton("Keyboard_Select");
	}

	private void ScanForKeyboard()
	{
		if (Input.GetButtonDown("Keyboard_Menu") || Input.GetButtonDown("Keyboard_Select"))
		{
			SetNextPlayer(PlayerInputMethod.Keyboard);
		}
	}

	#endregion

	#region Controller Support

	private void UpdateControllerInput(ref PlayerInput playerInput, int controllerID)
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		// Try and get the state
		PlayerIndex playerIndex = (PlayerIndex)controllerID;
		GamePadState testState = GamePad.GetState(playerIndex);

		if (!testState.IsConnected) // Check if connected
		{
			return;
		}

		playerInput.HorizontalMovement = testState.ThumbSticks.Left.X;
		playerInput.VerticalMovement = testState.ThumbSticks.Left.Y;
		playerInput.HorizontalAim = testState.ThumbSticks.Right.X;
		playerInput.VerticalAim = testState.ThumbSticks.Right.Y;

		playerInput.PushTransmission = testState.Triggers.Right != 0;
		playerInput.PullTransmission = testState.Triggers.Left != 0;

		playerInput.Menu = testState.Buttons.Start == ButtonState.Pressed;
		playerInput.Select = testState.Buttons.A == ButtonState.Pressed;
#endif
	}

	private void ScanForControllers()
	{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			PlayerIndex playerIndex = (PlayerIndex)i;
			GamePadState testState = GamePad.GetState(playerIndex);
			if (testState.IsConnected) // Check if connected
			{
				if (testState.Buttons.Start == ButtonState.Pressed)
					SetNextPlayer((PlayerInputMethod)(i + 2));
			}
			else
				DisconnectController((PlayerInputMethod)(i + 2));
		}
#endif
	}

	private void DisconnectController(PlayerInputMethod playerInputMethod)
	{
		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			if (playerInputMethods[i] == playerInputMethod)
			{
				playerInputMethods[i] = PlayerInputMethod.None;
			}
		}
	}

	#endregion

	private void DisconnectInvalidInputMethods()
	{
		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			if (playerInputMethods[i] == PlayerInputMethod.None)
			{
				playerInputs[i].InputMethod = PlayerInputMethod.None;

				playerInputs[i].HorizontalMovement = 0;
				playerInputs[i].VerticalMovement = 0;
				playerInputs[i].HorizontalAim = 0;
				playerInputs[i].VerticalAim = 0;

				playerInputs[i].PushTransmission = false;
				playerInputs[i].PullTransmission = false;

				playerInputs[i].Menu = false;
				playerInputs[i].Select = false;

				break;
			}
		}
	}
}