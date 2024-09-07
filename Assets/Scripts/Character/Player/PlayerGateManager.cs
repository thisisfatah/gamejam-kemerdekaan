using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadioRevolt
{
	public class PlayerGateManager : MonoBehaviour
	{
		[SerializeField] private List<PlayerData> _players = new List<PlayerData>();

		public PlayerData playerData { get; private set; }

		public void Init()
		{
			int rand = Random.Range(0, _players.Count);
			playerData = _players[rand];

			transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = playerData.characterSprite;
			transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = playerData.animator;
		}
	}
}

