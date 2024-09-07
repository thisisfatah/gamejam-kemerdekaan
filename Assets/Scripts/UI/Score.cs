using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RadioRevolt
{
	public class Score : MonoBehaviour
	{
		private Player _player;

		private void Start()
		{
			_player = FindObjectOfType<Player>();
		}

		private void Update()
		{
			if (_player.isGameOver) return;

			GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Score").ToString();
		}
	}
}
