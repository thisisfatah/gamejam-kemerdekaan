using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScene : BaseScene
{
	private void Start()
	{
		AudioManager.Instance.PlaySound("Api");
	}

	public void LoadLevel()
	{
		AudioManager.Instance.StopSound("Api");
		Transition.LoadLevel("GameScene", 1.0f, Color.black);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
