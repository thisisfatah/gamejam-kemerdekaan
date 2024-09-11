using UnityEngine;

namespace RadioRevolt
{
	public class GameScene : BaseScene
	{
		private bool isPause = false;
		public bool IsGameOver {  get; private set; }

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
					OpenPopup<PausePopup>("Popups/PausePopup", popup => popup.onClose.AddListener(() => { isPause = false; Time.timeScale = 1; }));
				}
				else
				{
					CloseCurrentPopup();
				}
			}
		}

		public void EndGame(bool win)
		{
			IsGameOver = true;
			OpenPopup<GameOverPopUp>("Popups/GameOverPopup", popup => popup.EndGame(win));
		}
	}
}

