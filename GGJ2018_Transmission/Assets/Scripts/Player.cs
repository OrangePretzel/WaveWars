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
void Awake()
 {
     rb = GetComponent<Rigidbody2D>();
     GameManager.Instance.InitGameManager(this);
     charge=100;
     transmission=false;
 }

void Update()
{
  playerInput = InputManager.GetPlayerInput(playerID);
  rb.velocity = new Vector2(playerInput.HorizontalMovement*speed, playerInput.VerticalMovement*speed);
  rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
  var playerPosition = this.transform.position;
  var ray = new Vector3(playerInput.HorizontalAim, playerInput.VerticalAim, 0);
  //Debug.Log(ray);

  if (transmission){
    Debug.DrawRay(playerPosition, ray, Color.yellow, 5.0f, true);
    charge--;
  }
  else if (!transmission && charge<100){
    charge++;
  }

  Debug.Log("Charge: "+charge);
  //Debug.Log("Pull: "+playerInput.PullTransmission+", Push: "+playerInput.PushTransmission);
  HandleTransmission();
  Debug.Log("Transmission: "+transmission);
}

void HandleTransmission(){
  // currently nothing, switch to push
  if (!transmission && playerInput.PushTransmission && charge>0 && Aiming()){
    transmission=true;
  }
  // currently nothing, switch to pull
  else if (!transmission && playerInput.PullTransmission && charge>0 && Aiming()){
    transmission=true;
  }
  else if (!playerInput.PushTransmission && !playerInput.PullTransmission && Aiming()){
    transmission=false;
  }
  else if (transmission && charge<=0){
    transmission=false;
  }
}

bool Aiming(){
  if (playerInput.HorizontalAim != 0 || playerInput.VerticalAim != 0)
  {
    return true;
} else { return false; }

}
}
