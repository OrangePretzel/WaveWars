using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	public Rigidbody2D rb;
	public float speed;
	public int playerID = 0;

	private int transmission;
	private PlayerInput playerInput;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		playerInput = InputManager.GetPlayerInput(playerID);
		var aimVec = playerInput.GetNormalizedAim(transform.position, Camera.main);

		rb.velocity = new Vector2(playerInput.HorizontalMovement * speed, playerInput.VerticalMovement * speed);
		rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
		var playerPosition = this.transform.position;
		var ray = new Vector3(aimVec.x, aimVec.y, 0);

		if (transmission == 1)
		{
			Debug.DrawRay(playerPosition, ray, Color.yellow, 5.0f, true);
		}
		else if (transmission == 2)
		{
			Debug.DrawRay(playerPosition, ray, Color.blue, 5.0f, true);
		}

		//Debug.Log("Pull: "+playerInput.PullTransmission+", Push: "+playerInput.PushTransmission);
		HandleTransmission();
	}

	void HandleTransmission()
	{
		// currently nothing, switch to push
		if (transmission == 0 && playerInput.PushTransmission)
		{
			transmission = 1;
		}
		// currently nothing, switch to pull
		else if (transmission == 0 && playerInput.PullTransmission)
		{
			transmission = 2;
		}
		else if (!playerInput.PushTransmission && !playerInput.PullTransmission)
		{
			transmission = 0;
		}
	}
}