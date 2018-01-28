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
void Awake()
 {
     rb = GetComponent<Rigidbody2D>();
     GameManager.Instance.InitGameManager(this);
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
  }
  else if (transmission==2){
    Debug.DrawRay(playerPosition, ray, Color.blue, 5.0f, true);
  }

  //Debug.Log("Pull: "+playerInput.PullTransmission+", Push: "+playerInput.PushTransmission);
  HandleTransmission();
  Debug.Log("Transmission set to: "+transmission);
}

void HandleTransmission(){
  // currently nothing, switch to push
  if (transmission==0 && playerInput.PushTransmission){
    transmission=1;
  }
  // currently nothing, switch to pull
  else if (transmission==0 && playerInput.PullTransmission){
    transmission=2;
  }
  else if (!playerInput.PushTransmission && !playerInput.PullTransmission){
    transmission=0;
  }
}

}
