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
		score.text = PlayerPrefs.GetInt("Score").ToString();
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
