using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectScreen : MonoBehaviour
{
	public Image[] PlayerIcons = new Image[4];
	public float[] PlayerIconTargetLocation = new float[4];

	public void AssignPlayerToTeam(int playerID, int teamID)
	{
		PlayerIconTargetLocation[playerID] = teamID == Entity.TEAM_A_ID ? -1 : 1;
	}

	private void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			var playerIcon = PlayerIcons[i];
			var targetPos = PlayerIconTargetLocation[i];

			
		}
	}
}