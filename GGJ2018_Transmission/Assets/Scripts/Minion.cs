using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	[SerializeField]
	private GameObject graphic;
	private float graphicInitialScaleX;

	private new Rigidbody2D rigidbody;
	[SerializeField]
	private Vector2 waveAffect;

	private Animator animator;

	public Canvas healthbarCanvas;
	public Image healthbar;
	public ParticleSystem deathParticles;

	private void OnEnable()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		hp = maxHP;
		healthbarCanvas.enabled = false;
		healthbar.fillAmount = 1;
		graphicInitialScaleX = graphic.transform.localScale.x;
	}

	private void Update()
	{
		if (GameManager.IsGamePaused)
			return;

		UpdateMinion();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Wall")
		{
			ContactPoint2D[] contactPoints = new ContactPoint2D[1];
			var contactCounts = collision.GetContacts(contactPoints);
			if (contactCounts == 0)
			{
				BounceBack((transform.position - collision.transform.position) * CollisionBounceMultiplier);
			}
			else
			{
				BounceBack(contactPoints[0].normal * CollisionBounceMultiplier);
			}
			return;
		}

		CollideWithEntity(collision);

	}

	private void CollideWithEntity(Collision2D collision)
	{
		if (!(IsAffectedByWave || AlwaysAffectedByWave))
			return;

		var otherEntity = collision.gameObject.GetComponent<Entity>();
		if (otherEntity == null)
			return;

		var bounceBack = (Vector2)(transform.position - otherEntity.transform.position) * CollisionBounceMultiplier;
		BounceBack(bounceBack);
	}

	private void BounceBack(Vector2 direction)
	{
		rigidbody.velocity += direction;
		IgnoreMaxSpeedHack = IGNORE_MAX_SPEED_HACK_DURATION;
		graphic.transform.localScale = new Vector3(Mathf.Sign(-direction.x) * graphicInitialScaleX, graphic.transform.localScale.y, 1);
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
		if (this.hp <= 0)
		{
			Instantiate(deathParticles, transform.position, new Quaternion());
			GameManager.Instance.MinionAudioSource.Play();
			GameManager.RemoveEntity(this);
			Destroy(this.gameObject);
		}

		//Debug.DrawLine(transform.position, transform.position + (Vector3)rigidbody.velocity);
		if (IsAffectedByWave || AlwaysAffectedByWave)
		{
			var enemy = GetNearbyEnemyEntity();
			if (enemy == null)
			{
				rigidbody.velocity += waveAffect;
				animator.SetBool("IsAttacking", false);
			}
			else
			{
				var enemyDir = enemy.transform.position - transform.position;
				rigidbody.velocity += (Vector2)enemyDir;
				this.Attack(enemy);
			}
			animator.SetBool("IsWalking", true);

			if (IsAffectedByWave)
			{
				AffectedByPlayerIDs.Clear();
				IsAffectedByWave = false;
				waveAffect = Vector2.zero;
			}

			if (IgnoreMaxSpeedHack > 0)
			{
				IgnoreMaxSpeedHack -= Time.deltaTime;
				return;
			}

			graphic.transform.localScale = new Vector3(Mathf.Sign(rigidbody.velocity.x) * graphicInitialScaleX, graphic.transform.localScale.y, 1);

			// Cap the speed to max speed
			if (rigidbody.velocity.sqrMagnitude > MaxSpeedSqr)
			{
				rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
			}
		}
		else
		{
			animator.SetBool("IsWalking", false);
			animator.SetBool("IsAttacking", false);
			rigidbody.velocity = Vector2.zero;
		}
	}

	private void Attack(Entity other)
	{
		animator.SetBool("IsAttacking", true);
		other.TakeDamage(damage * Time.deltaTime * AffectedByPlayerIDs.Count);
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

	public override void UpdateHealthBar()
	{
		if (maxHP > hp)
		{
			healthbarCanvas.enabled = true;
			healthbar.fillAmount = hp / maxHP;
		}
		else
		{
			healthbarCanvas.enabled = false;
			healthbar.fillAmount = 1;
		}
	}
}
