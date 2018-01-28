using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
public Rigidbody2D rb;
public float speed;
public int playerID = 0;
private bool transmission;
private PlayerInput playerInput;
private int charge;
private Vector3 ray;
private Vector3 playerPosition;
void Awake()
 {
     rb = GetComponent<Rigidbody2D>();
     GameManager.Instance.InitGameManager(this);
     charge=100;
     transmission=false;
     playerInput = InputManager.GetPlayerInput(playerID);
     ray = new Vector3(playerInput.HorizontalAim, playerInput.VerticalAim, 0);
 }

void Update()
{
  playerInput = InputManager.GetPlayerInput(playerID);
  rb.velocity = new Vector2(playerInput.HorizontalMovement*speed, playerInput.VerticalMovement*speed);
  rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
  playerPosition = this.transform.position;
  ray = new Vector3(playerInput.HorizontalAim, playerInput.VerticalAim, 0);

  HandleTransmission();
  Debug.Log("Transmission: "+transmission);
  ChargeWhileIdle();
  Debug.Log("Charge: "+charge);
}

<<<<<<< HEAD
void HandleTransmission(){
  // currently nothing, switch to push
  if ((playerInput.PushTransmission || playerInput.PullTransmission) && charge>0 && Aiming()){
    transmission=true;
    charge--;
    Debug.DrawRay(playerPosition, ray, Color.yellow, 5.0f, true);
  }
  else if (!playerInput.PushTransmission && !playerInput.PullTransmission && Aiming()){
    transmission=false;
  }
  else if (transmission && charge<=0){
    transmission=false;
    Debug.Log("CHARGE LESS THAN OR EQUAL TO ZERO");
  }
}
=======
		rb.velocity = new Vector2(playerInput.HorizontalMovement * speed, playerInput.VerticalMovement * speed);
		rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
		var playerPosition = this.transform.position;
		var ray = new Vector3(aimVec.x, aimVec.y, 0);
>>>>>>> 83c55e397137dee04d6f0604c9222099f76ca1ee

void ChargeWhileIdle(){
  if (!transmission && charge<100){
    charge++;
  }
}

<<<<<<< HEAD
bool Aiming(){
  if (playerInput.HorizontalAim != 0 || playerInput.VerticalAim != 0)
  {
    return true;
} else { return false; }
=======
		//Debug.Log("Pull: "+playerInput.PullTransmission+", Push: "+playerInput.PushTransmission);
		HandleTransmission();
	}
>>>>>>> 83c55e397137dee04d6f0604c9222099f76ca1ee

}
}
