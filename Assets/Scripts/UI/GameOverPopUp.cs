using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPopUp : Popup
{
    [SerializeField] TextMeshProUGUI score;

	protected override void Start()
	{
        base.Start();

		int Seconds = (int)(PlayerPrefs.GetFloat("Time") % 60);
		int Minutes = (int)(PlayerPrefs.GetFloat("Time") / 60);

		score.text = string.Format("{0:00}:{1:00}", Minutes, Seconds);
	}

	private void Update()
    {
        if (Input.anyKeyDown)
        {
            AudioManager.Instance.StopSound("DarkForest");
            Transition.LoadLevel("MenuScene", 1.0f, Color.black);
        }
    }
}
