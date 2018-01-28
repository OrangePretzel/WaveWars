using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectScreen : MonoBehaviour
{
	private Canvas canvas;
	public Canvas BalancedTeams;

	public float TeamOffset = 1;
	public float TransitionSpeed = 5;

	public Image[] PlayerIcons = new Image[4];
	public float[] PlayerIconTargetLocation = new float[4];
	public bool[] PlayerEnabled = new bool[4];
	public bool[] PlayerReady = new bool[4];

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	public void AssignPlayerToTeam(int playerID, int teamID)
	{
		PlayerIconTargetLocation[playerID] = teamID == Entity.TEAM_A_ID ? -1 : 1;
	}

	public void UnAssignPlayerFromTeams(int playerID)
	{
		PlayerReady[playerID] = false;
		PlayerIconTargetLocation[playerID] = 0;
	}

	public void MakePlayerReady(int playerID, bool status)
	{
		if (PlayerIconTargetLocation[playerID] != 0)
			PlayerReady[playerID] = status;
	}

	public bool AnyPlayerConnected()
	{
		return PlayerEnabled[0] || PlayerEnabled[1] || PlayerEnabled[2] || PlayerEnabled[3];
	}

	public bool AreAllPlayersReady()
	{
		var p1Ready = (PlayerEnabled[0] && PlayerReady[0]) || !PlayerEnabled[0];
		var p2Ready = (PlayerEnabled[1] && PlayerReady[1]) || !PlayerEnabled[1];
		var p3Ready = (PlayerEnabled[2] && PlayerReady[2]) || !PlayerEnabled[2];
		var p4Ready = (PlayerEnabled[3] && PlayerReady[3]) || !PlayerEnabled[3];
		return p1Ready && p2Ready && p3Ready && p4Ready;
	}

	public bool IsPlayerReady(int playerID)
	{
		return PlayerReady[playerID];
	}

	private void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			var playerIcon = PlayerIcons[i];

			if (PlayerEnabled[i])
			{
				if (PlayerReady[i])
					playerIcon.color = Color.green;
				else
					playerIcon.color = Color.white;
			}
			else
			{
				playerIcon.color = Color.gray;
			}

			var targetPos = PlayerIconTargetLocation[i] * TeamOffset;
			var currentPos = playerIcon.rectTransform.localPosition.x;

			var deltaPos = (targetPos - currentPos) * TransitionSpeed * Time.unscaledDeltaTime;
			playerIcon.rectTransform.localPosition = new Vector3(currentPos + deltaPos, 0);
		}
	}

	public void SetBalanced(bool isBalanced)
	{
		BalancedTeams.enabled = !isBalanced;
	}

	public void ToggleScreen(bool toggle)
	{
		canvas.enabled = toggle;
	}

	public void SetPlayerEnabled(int playerID, bool isEnabled)
	{
		PlayerEnabled[playerID] = isEnabled;
		if (!isEnabled)
		{
			UnAssignPlayerFromTeams(playerID);
		}
	}
}