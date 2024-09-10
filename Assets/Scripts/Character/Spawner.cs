using DG.Tweening;
using RadioRevolt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RadioRevolt
{
	public class Spawner : MonoBehaviour
	{
		[System.Serializable]
		public class Wave
		{
			public List<ObjectGroup> objectGroups = new List<ObjectGroup>();
			public float spawnInteval;

			[HideInInspector] public int waveQuota;
			[HideInInspector] public int spawnCount;
		}

		[System.Serializable]
		public class ObjectGroup
		{
			public string objectName;
			public GameObject enemyPrefab;
			public int objectCount;

			[HideInInspector] public int spawnCount;
		}

		[SerializeField] private Wave wave;

		[SerializeField] private Transform spawnPoint;
		private float spawnTimer;

		private void Start()
		{
			CalculateWaveQuota();

			SpawnObjects();
		}

		private void Update()
		{
			if (wave.spawnCount <= wave.waveQuota)
			{
				spawnTimer += Time.deltaTime;
				if (spawnTimer >= wave.spawnInteval)
				{
					spawnTimer = 0;
					SpawnObjects();
				}
			}
		}

		private void CalculateWaveQuota()
		{
			int currentWaveQuota = 0;
			foreach (ObjectGroup enemyGroup in wave.objectGroups)
			{
				currentWaveQuota += enemyGroup.objectCount;
			}

			wave.waveQuota = currentWaveQuota;
		}

		private void SpawnObjects()
		{
			foreach (var objGroup in wave.objectGroups)
			{
				if (objGroup.spawnCount <= objGroup.objectCount)
				{
					ObjectPoolManager.SpawnObject(objGroup.enemyPrefab, spawnPoint);
					objGroup.spawnCount++;
				}
			}
			wave.spawnCount++;
			FormatCharacter();
		}

		protected void FormatCharacter()
		{
			for (int i = 0; i < spawnPoint.childCount; i++)
			{
				float x = 1 * Mathf.Sqrt(i) * Mathf.Cos(i * 0.833f);
				float y = 1 * Mathf.Sqrt(i) * Mathf.Sin(i * 0.833f);

				Vector2 newPos = new Vector2(x, y);
				spawnPoint.GetChild(i).DOLocalMove(newPos, 0.5f).SetEase(Ease.OutBack);

				if (i == 0) continue;
			}
		}
	}
}
