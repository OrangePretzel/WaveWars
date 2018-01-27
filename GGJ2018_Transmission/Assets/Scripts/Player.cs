using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
public Rigidbody2D rb;
public float speed;

void Awake()
 {
     rb = GetComponent<Rigidbody2D>();
     GameManager.Instance.InitGameManager(this);
 }

void Update()
{
  float moveHorizontal = Input.GetAxis("Horizontal");
  float moveVertical = Input.GetAxis("Vertical");
  rb.velocity = new Vector2(moveHorizontal * speed, moveVertical * speed);
}

}
