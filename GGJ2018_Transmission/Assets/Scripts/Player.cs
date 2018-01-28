using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
public Rigidbody2D rb;
public float speed;
public int playerID = 0;
private int transmission = 0;
private PlayerInput playerInput;
private int charge;
void Awake()
 {
     rb = GetComponent<Rigidbody2D>();
     GameManager.Instance.InitGameManager(this);
     charge=100;
 }

void Update()
{
  playerInput = InputManager.GetPlayerInput(playerID);
  rb.velocity = new Vector2(playerInput.HorizontalMovement*speed, playerInput.VerticalMovement*speed);
  rb.MovePosition(rb.position + rb.velocity * Time.deltaTime);
  var playerPosition = this.transform.position;
  var ray = new Vector3(playerInput.HorizontalAim, playerInput.VerticalAim, 0);
  //Debug.Log(ray);

  if (transmission==1){
    Debug.DrawRay(playerPosition, ray, Color.yellow, 5.0f, true);
    charge--;
  }
  else if (transmission==2){
    Debug.DrawRay(playerPosition, ray, Color.blue, 5.0f, true);
    charge--;
  }
  else if (transmission==0 && charge<100){
    charge++;
  }

  Debug.Log("Charge: "+charge);
  //Debug.Log("Pull: "+playerInput.PullTransmission+", Push: "+playerInput.PushTransmission);
  HandleTransmission();
  Debug.Log("Transmission set to: "+transmission);
}

void HandleTransmission(){
  // currently nothing, switch to push
  if (transmission==0 && playerInput.PushTransmission && charge>0){
    transmission=1;
  }
  // currently nothing, switch to pull
  else if (transmission==0 && playerInput.PullTransmission && charge>0){
    transmission=2;
  }
  else if (!playerInput.PushTransmission && !playerInput.PullTransmission){
    transmission=0;
  }
  else if (transmission==1 && charge<0){
    transmission=0;
  }
  else if (transmission==2 && charge<0){
    transmission=0;
  }
}

}
