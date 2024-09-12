using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RadioRevolt
{
	public class Score : MonoBehaviour
	{
		private GameScene gameScene;

		private void Start()
		{
			gameScene = FindObjectOfType<GameScene>();

			PlayerPrefs.SetFloat("Time", 0);
		}

		private void Update()
		{
			if (gameScene.IsGameOver) return;

			float time = PlayerPrefs.GetFloat("Time");
			time += Time.deltaTime;
			PlayerPrefs.SetFloat("Time", time);

			/*  */
			//int Seconds = (int)(PlayerPrefs.GetFloat("Time") % 60);
			//int Minutes = (int)(PlayerPrefs.GetFloat("Time") / 60);
			
			//GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", Minutes, Seconds);
		}
	}
}
