using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
// This is the single instance of the game manager
public static GameManager Instance { get; private set; }
// UI Helper
public UIHelper UIHelper;

public void InitGameManager(Player p)
  {
    UIHelper = GameObject.Find("Camera UI").GetComponent<UIHelper>();
  }

public class Stats
{

}

  public static Stats GameStats;
  // If true we are paused
  public bool IsPaused;

// Awake is fired BEFORE Start
void Awake()
{
    // No instance yet
    if (Instance == null)
    {
        // This is the instance
        Instance = this;
        DontDestroyOnLoad(gameObject);
        GameStats = new Stats();
    }
    // There is an instance already
    else if (Instance != this)
    {
        // Destroy this
        Destroy(gameObject);
        return;
    }
}

    private void Update()
    {
      // if (!UIHelper.IsInDialog && Input.GetKeyDown(KeyCode.P))
      // {
      //   TriggerPauseMenu();
      // }
    }

    private void TriggerPauseMenu()
      {
      }

  // Toggle the pause
  public void TogglePause()
  {
      // Self explanatory?
      if (IsPaused)
          UnpauseGame();
      else
          PauseGame();
  }

  // Time.timescale controls the update rate of the game. 0 is paused, 1 is normal, in between is slow motion and greater than 1 is fast
  // Pause the game
  public void PauseGame()
  {
      // Pause the game
      Time.timeScale = 0;
      IsPaused = true;
  }

  // Unpause the game
  public void UnpauseGame()
  {
      // Unpause the game
      Time.timeScale = 1;
      IsPaused = false;
  }

}
