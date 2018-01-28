using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Minion : Entity
{
	private const float NEARBY_ENEMY_DIST_CUTOFF = 2f;
	private const float NEARBY_ENEMY_DIST_CUTOFF_SQR = NEARBY_ENEMY_DIST_CUTOFF * NEARBY_ENEMY_DIST_CUTOFF;
	private const float IGNORE_MAX_SPEED_HACK_DURATION = 1;

	public float MaxSpeed = 5;
	public float MaxSpeedSqr => MaxSpeed * MaxSpeed;

	public float CollisionBounceMultiplier = 2;

	public bool AlwaysAffectedByWave = false;

	public bool IsAffectedByWave = false;
	private List<int> AffectedByPlayerIDs = new List<int>();

	private float IgnoreMaxSpeedHack = 0;

	private new Rigidbody2D rigidbody;
	[SerializeField]
	private Vector2 waveAffect;

	[SerializeField]
	private int hp, damage;

	private void OnEnable()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		hp=100;
		damage=5;
	}

	private void Update()
	{
		UpdateMinion();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var otherEntity = collision.gameObject.GetComponent<Entity>();
		if (otherEntity == null)
			return;

		if (otherEntity.TeamID != TeamID)
		{
			var bounceBack = (Vector2)(transform.position - otherEntity.transform.position) * CollisionBounceMultiplier;
			rigidbody.velocity += bounceBack;
			IgnoreMaxSpeedHack = IGNORE_MAX_SPEED_HACK_DURATION;
			this.Attack((Minion)otherEntity);
			Debug.Log("reached Attack");
		}

	}

	public void AffectByWave(Vector2 wave, int playerID, float strength = 1)
	{
		if (AffectedByPlayerIDs.Contains(playerID))
			return;
		AffectedByPlayerIDs.Add(playerID);
		IsAffectedByWave = true;

		waveAffect += wave;
	}

	private void UpdateMinion()
	{
		// Delete minion from scene if HP drops to/below zero
		if (this.hp<=0){
			Debug.Log("destroying minion");
			Destroy(this.gameObject);
		}

		Debug.DrawLine(transform.position, transform.position + (Vector3)rigidbody.velocity);
		if (IsAffectedByWave || AlwaysAffectedByWave)
		{
			var enemy = GetNearbyEnemyEntity();
			if (enemy == null)
			{
				rigidbody.velocity += waveAffect;
			}
			else
			{
				var enemyDir = enemy.transform.position - transform.position;
				rigidbody.velocity += (Vector2)enemyDir;

			}

			if (IgnoreMaxSpeedHack > 0)
			{
				IgnoreMaxSpeedHack -= Time.deltaTime;
				return;
			}

			// Cap the speed to max speed
			if (rigidbody.velocity.sqrMagnitude > MaxSpeedSqr)
			{
				rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
			}

			IsAffectedByWave = false;
			AffectedByPlayerIDs.Clear();
			//waveAffect = Vector2.zero;
		}
	}

	private void Attack(Minion other)
	{
		other.hp-=damage;
		Debug.Log("Attacked the other minion!!! Their HP: "+other.hp);
	}

	private Entity GetNearbyEnemyEntity()
	{
		var enemies = GameManager.GetEnemyEntities(TeamID);

		foreach (var enemy in enemies)
		{
			var enemyDir = enemy.transform.position - transform.position;
			// if there's an enemy within the cutoff radius:
			if (enemyDir.sqrMagnitude <= NEARBY_ENEMY_DIST_CUTOFF_SQR)
				return enemy;
		}
		// no enemy within radius
		return null;
	}
}
