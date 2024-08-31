using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopup : Popup
{
	public void Resume()
	{
		Close();
	}

	public void Restart()
	{
		Time.timeScale = 1;
		Transition.LoadLevel("GameScene", 1.0f, Color.black);
	}

	public void Quit()
	{
		Transition.LoadLevel("MenuScene", 1.0f, Color.black);
	}
}
