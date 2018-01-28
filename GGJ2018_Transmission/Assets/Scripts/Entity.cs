using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
	public const int TEAM_A_ID = 0;
	public const int TEAM_B_ID = 1;

	public int TeamID { get { return teamID; } }
	public int PlayerID
	{
		get { return playerID; }
		set { playerID = value; }
	}
	public bool Reflective { get { return reflective; } }

	public float maxHP;
	public float hp;
	public float damage;

	[SerializeField]
	protected int teamID = -1;
	[SerializeField]
	protected int playerID = -1;
	[SerializeField]
	protected bool reflective = false;

	public void SetEntityProperties(int teamID, int playerID, bool reflective)
	{
		this.teamID = teamID;
		this.playerID = playerID;
		this.reflective = reflective;
	}

	public void TakeDamage(float damage)
	{
		hp -= damage;
		UpdateHealthBar();
	}

	public virtual void UpdateHealthBar() { }
}
