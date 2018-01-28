using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity
{
	public float TransmittingVelocityModifier = 0.75f;

	public new Rigidbody2D rigidbody;
	public float speed;
	private PlayerInput playerInput;
	private WaveEmitter waveEmitter;

	public float MaxCharge = 10;

	[SerializeField]
	private float charge;

	public Canvas healthbarCanvas;
	public Image healthbar;
	public ParticleSystem deathParticles;

	void Awake()
	{
		hp = maxHP;
		healthbarCanvas.enabled = false;
		rigidbody = GetComponent<Rigidbody2D>();
		charge = MaxCharge;
		playerInput = InputManager.GetPlayerInput(PlayerID);
		waveEmitter = GetComponentInChildren<WaveEmitter>();
		waveEmitter?.SetEntityProperties(teamID, playerID, reflective);
	}

	void Update()
	{
		if (GameManager.IsGamePaused)
			return;

		// Check if the player should be dead.
		if (hp <= 0)
		{
			Instantiate(deathParticles, transform.position, new Quaternion());
			gameObject.SetActive(false);
			// Do stuff for respawn.
		}

		playerInput = InputManager.GetPlayerInput(playerID);

		var aimVec = playerInput.GetNormalizedAim(transform.position, Camera.main);
		aimVec.x *= transform.localScale.x;
		bool isTransmitting = aimVec.sqrMagnitude != 0
			&& (playerInput.PlayerDevice != InputManager.KEYBOARD_AND_MOUSE || Input.GetKey(KeyCode.Mouse0));

		rigidbody.velocity = new Vector2(playerInput.HorizontalMovement, playerInput.VerticalMovement)
			* speed
			* (isTransmitting ? TransmittingVelocityModifier : 1);
		rigidbody.MovePosition(rigidbody.position + rigidbody.velocity * Time.deltaTime);

		if (rigidbody.velocity.x != 0)
		{
			var dir = Mathf.Sign(rigidbody.velocity.x);
			transform.localScale = new Vector3(dir, 1, 1);
		}

		if (isTransmitting && charge > 0)
		{
			if (!GameManager.GameSettings.UnlimitedTransmission)
				charge -= Time.deltaTime;

			ShootTransmission(aimVec);
			//Debug.DrawRay(playerPosition, aimVec, Color.yellow, 5.0f, true);
		}

		if (!isTransmitting || charge <= 0)
		{
			waveEmitter?.StopTransmit();
		}

		if (!isTransmitting && charge < MaxCharge)
		{
			charge += Time.deltaTime;
			if (charge >= MaxCharge)
				charge = MaxCharge;
		}
	}

	private void ShootTransmission(Vector3 direction)
	{
		if (waveEmitter)
		{
			var q = new Quaternion();
			q.SetLookRotation(Vector3.forward, direction);
			waveEmitter.transform.localRotation = q;
			waveEmitter.Transmit();
		}
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
