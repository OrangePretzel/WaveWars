using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
	private Canvas canvas;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	public void ResumeGame()
	{
		GameManager.Instance.UnpauseGame();
		GameManager.Instance.HideMenuScreen();
	}

	public void PickPlayers()
	{
		GameManager.Instance.HideMenuScreen();
		GameManager.QuickResetPlayers();
		GameManager.Instance.ShowPlayerSelectScreen();
	}

	public void NewGame()
	{
		GameManager.NewGame();
	}

	public void QuitGame()
	{
		if (!Application.isEditor)
			Application.Quit();
		else
			Debug.Log("No quitty quits 4 u");
	}

	internal void ToggleScreen(bool v)
	{
		// not enought time to rename the variable
		// but enough to leave 2 comments ;)
		canvas.enabled = v;
	}
}