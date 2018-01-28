using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
  public Player spawnA;
  public Player spawnB;
  public Player spawnX;
  public Player spawnY;

  void Awake() {
    InvokeRepeating ("Spawn", 0f, spawnDelay);
  }

  void SpawnDead() {

  }
}
