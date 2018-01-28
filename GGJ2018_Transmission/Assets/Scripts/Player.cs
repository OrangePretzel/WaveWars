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

void ChargeWhileIdle(){
  if (!transmission && charge<100){
    charge++;
  }
}

bool Aiming(){
  if (playerInput.HorizontalAim != 0 || playerInput.VerticalAim != 0)
  {
    return true;
} else { return false; }

}
}
