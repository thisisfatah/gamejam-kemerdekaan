
using UnityEngine;

public class GameLosePopup : Popup
{
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			Transition.LoadLevel("MenuScene", 1.0f, Color.black);
		}
	}
}
