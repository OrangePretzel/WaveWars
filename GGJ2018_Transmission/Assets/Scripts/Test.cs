using UnityEngine;

public class Test : MonoBehaviour
{
	public InputManager InputManager;
	private PlayerInput[] playerInput = new PlayerInput[4];

	private void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (InputManager.GetPlayerInput(i, ref playerInput[i]))
			{
			}
		}
	}

	private void OnGUI()
	{
		const bool DEBUG_GUI = true;
		if (DEBUG_GUI)
		{
			GUI.Label(new Rect(0, 0, 1000, 1000), $@"
Player 1:
{playerInput[0]}

Player 2:
{playerInput[1]}

Player 3:
{playerInput[2]}

Player 4:
{playerInput[3]}
");
		}
	}
}