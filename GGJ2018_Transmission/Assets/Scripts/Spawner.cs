using UnityEngine;

public class Spawner : MonoBehaviour
{
  public float spawnDelay = 5;
  public GameObject thingToSpawn;
  public GameObject minions;

  void Awake(){
    InvokeRepeating ("Spawn", 0f, spawnDelay);
  }

  void Update(){

  }

  void Spawn(){
    // spawn anything
    GameObject newMin = Instantiate(thingToSpawn, this.transform);
    Vector2 forceA = new Vector2(1,0);
    Vector2 forceB = new Vector2(-1,0);
    var MinionEntity = newMin.GetComponent<Entity>();
    var MinionType = newMin.GetComponent<Minion>();
    GameManager.AddEntity(MinionEntity);
    if (MinionEntity.TeamID==0) {
      MinionType.GetComponent<Rigidbody2D>().AddForce(forceA);
    } else {
      MinionType.GetComponent<Rigidbody2D>().AddForce(forceB);
    }

    Debug.Log("Spawn called at :"+this.transform.ToString());
  }
}
