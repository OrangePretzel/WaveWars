using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
public Rigidbody2D rb;
public float speed;
//public playerID id;

void Awake()
 {
     rb = GetComponent<Rigidbody2D>();
     GameManager.Instance.InitGameManager(this);
 }

void Update()
{
  var playerInput = InputManager.GetPlayerInput(0);
  rb.velocity = new Vector2(playerInput.HorizontalMovement*speed, playerInput.VerticalMovement*speed);
  rb.MovePosition(rb.position + rb.velocity * Time.fixedDeltaTime);
}

}
