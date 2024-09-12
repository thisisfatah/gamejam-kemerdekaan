using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RadioRevolt
{
	public class GameScene : BaseScene
	{
		private bool isPause = false;
		public bool IsGameOver {  get; private set; }


		[SerializeField] private TextMeshProUGUI bossCountText;
		[SerializeField] private TextMeshProUGUI miniBossCountText;

		public List<EnemyBehaviour> bossList;
		public List<EnemyBehaviour> miniBossList;

		private void Start()
		{
			AudioManager.Instance.PlaySound("DarkForest");
			PlayerPrefs.SetInt("Score", 0);
			IsGameOver = false;
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				isPause = !isPause;
				Time.timeScale = isPause ? 0 : 1;
				if (isPause)
				{
					AudioManager.Instance.StopSound("FootStep");
					OpenPopup<PausePopup>("Popups/PausePopup", popup => popup.onClose.AddListener(() => { 
						isPause = false; 
						Time.timeScale = 1; 
					}));
				}
				else
				{
					CloseCurrentPopup();
				}
			}

			bossCountText.text = bossList.Count.ToString() + "X";
			miniBossCountText.text = miniBossList.Count.ToString() + "X";

			if (!IsGameOver && bossList.Count <= 0 && miniBossList.Count <= 0)
			{
				EndGame(true);
			}
		}

		public void EndGame(bool win)
		{
			IsGameOver = true;

			if (win)
			{
				OpenPopup<GameOverPopUp>("Popups/GameWinPopup");
			}
			else
			{
				OpenPopup<GameLosePopup>("Popups/GameLosePopup");
			}
		}
	}
}

