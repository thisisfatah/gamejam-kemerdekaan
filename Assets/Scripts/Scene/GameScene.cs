using UnityEngine;

namespace RadioRevolt
{
	public class GameScene : BaseScene
	{
		private bool isPause = false;
		private void Start()
		{
			AudioManager.Instance.PlaySound("DarkForest");
			PlayerPrefs.SetInt("Score", 0);
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
	}
}

