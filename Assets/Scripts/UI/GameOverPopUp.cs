using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class GameOverPopUp : Popup
{
	[SerializeField] private TextMeshProUGUI nameLeaderBoard;
	[SerializeField] private TextMeshProUGUI scoreLeaderBoard;

	[SerializeField] private TMP_InputField scoreInput;

	[SerializeField] private Leaderboard leaderboard = new Leaderboard();

	private List<GameObject> showingScore = new List<GameObject>();

	private bool SavedFile = false;

	protected override void Start()
	{
		base.Start();

		LoadData();
	}

	private void LoadData()
	{
		if (File.Exists(Application.dataPath + "/save.txt"))
		{
			string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
			leaderboard = JsonUtility.FromJson<Leaderboard>(saveString);

			ShowLeaderBoard();
		}
	}

	public void SaveScore()
	{
		if (!SavedFile)
		{
			int Seconds = (int)(PlayerPrefs.GetFloat("Time") % 60);
			int Minutes = (int)(PlayerPrefs.GetFloat("Time") / 60);

			string name = scoreInput.textComponent.text;
			string score = string.Format("{0:00}:{1:00}", Minutes, Seconds);

			SavedFile = true;

			Leaderboard.ScoreGroup scoreGroup = new()
			{
				name = name,
				score = score
			};

			leaderboard.scoreGroups.Add(scoreGroup);

			string json = JsonUtility.ToJson(leaderboard);

			File.WriteAllText(Application.dataPath + "/save.txt", json);
			ShowLeaderBoard();

			StartCoroutine(DelayLoadScene());
		}
		else 
		{
			AudioManager.Instance.StopSound("DarkForest");
			Transition.LoadLevel("MenuScene", 1.0f, Color.black);
		}
	}

	private IEnumerator DelayLoadScene()
	{
		yield return new WaitForSeconds(3.0f);

		AudioManager.Instance.StopSound("DarkForest");
		Transition.LoadLevel("MenuScene", 1.0f, Color.black);
	}

	public void EndGame(bool win)
	{
		SavedFile = !win;
		scoreInput.gameObject.SetActive(win);
	}

	private void ShowLeaderBoard()
	{
		leaderboard.scoreGroups.Sort((left, right) => left.score.CompareTo(right.score));

		foreach (var showText in showingScore)
		{
			Destroy(showText);
		}
		showingScore.Clear();

		for (int i = 0; i < leaderboard.scoreGroups.Count; i++)
		{
			if (i <= 4)
			{
				TextMeshProUGUI name = Instantiate(nameLeaderBoard, transform);
				TextMeshProUGUI score = Instantiate(scoreLeaderBoard, transform);
				Vector3 namePosition = name.rectTransform.position;
				Vector3 scorePosition = score.rectTransform.position;
				namePosition.y -= i * 57;
				scorePosition.y -= i * 57;
				name.rectTransform.position = namePosition;
				score.rectTransform.position = scorePosition;

				int indexWin = i + 1;
				name.text = indexWin + " " + leaderboard.scoreGroups[i].name;
				score.text = leaderboard.scoreGroups[i].score;

				showingScore.Add(name.gameObject);
				showingScore.Add(score.gameObject);
			}
		}
	}
}
