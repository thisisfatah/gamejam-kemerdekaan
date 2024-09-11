using System.Collections.Generic;

[System.Serializable]
public class Leaderboard
{
	[System.Serializable]
	public class ScoreGroup
	{
		public string name;
		public string score;
	}

	public List<ScoreGroup> scoreGroups; 
}
