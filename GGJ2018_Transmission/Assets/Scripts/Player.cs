using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	public new Rigidbody2D rigidbody;
	public float speed;
	public int playerID = 0;
	private bool isTransmitting;
	private PlayerInput playerInput;

	public float MaxCharge = 10;

	[SerializeField]
	private float charge;

	private Vector3 playerPosition;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		charge = MaxCharge;
		isTransmitting = false;
		playerInput = InputManager.GetPlayerInput(playerID);
	}

	void Update()
	{
		playerInput = InputManager.GetPlayerInput(playerID);
		rigidbody.velocity = new Vector2(playerInput.HorizontalMovement * speed, playerInput.VerticalMovement * speed);
		rigidbody.MovePosition(rigidbody.position + rigidbody.velocity * Time.deltaTime);
		playerPosition = this.transform.position;

		HandleTransmission();
	}

	void HandleTransmission()
	{
		isTransmitting = playerInput.VerticalAim != 0 || playerInput.HorizontalAim != 0;

		if (isTransmitting && charge > 0)
		{
			charge -= Time.deltaTime;

			ShootTransmission(new Vector3(playerInput.HorizontalAim, playerInput.VerticalAim, 0));
			Debug.DrawRay(playerPosition, new Vector3(playerInput.HorizontalAim, playerInput.VerticalAim, 0), Color.yellow, 5.0f, true);
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

	}
}
