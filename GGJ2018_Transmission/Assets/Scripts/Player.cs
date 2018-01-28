using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	public Rigidbody2D rb;
	public float speed;
	public int playerID = 0;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		var playerInput = InputManager.GetPlayerInput(playerID);
		var aimVec = playerInput.GetNormalizedAim(transform.position, Camera.main);

		rb.velocity = new Vector2(playerInput.HorizontalMovement * speed, playerInput.VerticalMovement * speed);
		rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
		var playerPosition = this.transform.position;
		var ray = new Vector3(aimVec.x, aimVec.y, 0);
		Debug.Log(ray);
		Debug.DrawRay(playerPosition, ray, Color.yellow, 5.0f, true);
	}
}
