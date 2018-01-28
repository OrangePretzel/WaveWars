using UnityEngine;

public class Spawner : MonoBehaviour
{
  public float spawnDelay = 5;
  private Vector3 spawnerPos;

  void Awake(){
    spawnerPos = this.transform.position;
  }

  void Update(){
    
  }

  void Spawn(Entity e){
    // spawn anything
  }
}
