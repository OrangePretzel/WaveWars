using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
public Rigidbody2D rb;
public float speed;
public int playerID = 0;

void Awake()
 {
     rb = GetComponent<Rigidbody2D>();
     GameManager.Instance.InitGameManager(this);
 }

void Update()
{
  var playerInput = InputManager.GetPlayerInput(playerID);
  rb.velocity = new Vector2(playerInput.HorizontalMovement*speed, playerInput.VerticalMovement*speed);
  rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
  var playerPosition = this.transform.position;
  var ray = new Vector3(playerInput.HorizontalAim, playerInput.VerticalAim, 0);
  Debug.Log(ray);
  Debug.DrawRay(playerPosition, ray, Color.yellow, 5.0f, true);
}

}
