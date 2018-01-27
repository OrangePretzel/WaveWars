using UnityEngine;

public class PlayerInput
{
	public string PlayerDevice;
	public int PlayerID;

	public float HorizontalMovement;
	public float VerticalMovement;
	public float HorizontalAim;
	public float VerticalAim;

	public bool PushTransmission;
	public bool PullTransmission;

	public bool Menu;
	public bool Select;

	public override string ToString()
	{
		return $@"Player: ({PlayerID})
Input Device: ({PlayerDevice})
Movement: ({HorizontalMovement}, {VerticalMovement})
Aim: ({HorizontalAim}, {VerticalAim})
Push: ({PushTransmission})
Pull: ({PullTransmission})
Menu: ({Menu})
Select: ({Select})";
	}
}

public class InputManager : MonoBehaviour
{
	private const int MAX_PLAYERS = 4;

	private InControl.InputDevice[] playerDevices = new InControl.InputDevice[MAX_PLAYERS];
	private PlayerInput[] playerInputs = new PlayerInput[MAX_PLAYERS];

	#region Unity Hooks

	private void Awake()
	{
		ResetPlayerDevices();
		InControl.InputManager.OnDeviceAttached += inputDevice => { Debug.Log($"Device [{inputDevice.Name}] attached"); };
		InControl.InputManager.OnDeviceDetached += inputDevice =>
		{
			foreach (var playerDevice in playerDevices)
			{

			}
			Debug.Log($"Device [{inputDevice.Name}] detached");
		};
	}

	private void FixedUpdate()
	{
		foreach (var inputDevice in InControl.InputManager.Devices)
		{
			if (!inputDevice.IsAttached || !inputDevice.IsKnown || !inputDevice.IsSupportedOnThisPlatform)
				continue;

			if (inputDevice.Command)
			{
				SetNextPlayer(inputDevice);
			}
		}
	}

	private void Update()
	{
		UpdatePlayerInputs();
	}

	#endregion

	public PlayerInput GetPlayerInput(int playerID)
	{
		return playerInputs[playerID];
	}

	private void ResetPlayerDevices()
	{
		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			playerDevices[i] = null;
			playerInputs[i] = new PlayerInput() { PlayerID = i };
		}
	}

	private void SetNextPlayer(InControl.InputDevice inputDevice)
	{
		const int INVALID = -1;
		int potentialPlayerID = INVALID;

		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			if (playerDevices[i] == inputDevice)
				return;
			else if (playerDevices[i] == null && potentialPlayerID == INVALID)
				potentialPlayerID = i;
		}

		if (potentialPlayerID != INVALID)
		{
			Debug.Log($"Player {potentialPlayerID + 1} controller set to {inputDevice.Name}");
			playerDevices[potentialPlayerID] = inputDevice;
		}
	}

	private void UpdatePlayerInputs()
	{
		for (int i = 0; i < MAX_PLAYERS; i++)
		{
			var inputDevice = playerDevices[i];
			if (inputDevice == null)
				continue;

			var playerInput = playerInputs[i];
			playerInput.PlayerDevice = inputDevice.Name;

			playerInput.HorizontalMovement = inputDevice.LeftStick.X;
			playerInput.VerticalMovement = inputDevice.LeftStick.Y;

			playerInput.HorizontalAim = inputDevice.RightStick.X;
			playerInput.VerticalAim = inputDevice.RightStick.Y;

			playerInput.PushTransmission = inputDevice.RightTrigger;
			playerInput.PullTransmission = inputDevice.LeftTrigger;

			playerInput.Menu = inputDevice.Command;
			playerInput.Select = inputDevice.Action1;
		}
	}
}