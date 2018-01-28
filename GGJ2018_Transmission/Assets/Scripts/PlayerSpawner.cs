using UnityEngine;
public class PlayerSpawner : MonoBehaviour
{
  //public Player spawnMyPlayer;
  public int SECONDS_TIL_SPAWN = 10;
  void Awake() {
    //InvokeRepeating ("Spawn", 0f, spawnDelay);
  }

  void Spawn(Player spawnMe) {
    Invoke("ExecuteSpawn", SECONDS_TIL_SPAWN);
  }

  void ExecuteSpawn(Player spawnMe) {
    //if (playerID =)
    spawnMe.SetActive(true);
  }
}
