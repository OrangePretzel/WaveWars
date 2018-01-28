using UnityEngine;

public class Spawner : MonoBehaviour
{
  public float spawnDelay = 5;
  public GameObject thingToSpawn;

  void Awake(){
    InvokeRepeating ("Spawn", spawnDelay, spawnDelay);
  }

  void Update(){

  }

  void Spawn(){
    // spawn anything
    Instantiate(thingToSpawn, this.transform);
    Debug.Log("Spawn called");
  }
}
