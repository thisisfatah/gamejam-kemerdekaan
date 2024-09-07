using RadioRevolt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RadioRevolt
{
	public class EnemySpawner : MonoBehaviour
	{
		[System.Serializable]
		public class Wave
		{
			public string waveName;
			public List<EnemyGroup> enemyGroups = new List<EnemyGroup>();
			public int waveQuota;
			public float spawnInterfal;
			public int spawnCount;
		}

		[System.Serializable]
		public class EnemyGroup
		{
			public string enemyName;
			public int enemyCount;
			public int spawnCount;
			public GameObject enemyPrefab;
		}

		public List<Wave> waves;
		public int currentWaveCount;

		private Transform player;
		private float spawnTimer;

		[SerializeField] private GameObject alert;

		private void Start()
		{
			player = FindObjectOfType<PlayerManager>().transform;
			CalculateWaveQuota();
		}

		private void Update()
		{
			if (currentWaveCount < waves.Count - 1 && waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota)
			{
				BaginNextWave();
			}

			spawnTimer += Time.deltaTime;
			if (spawnTimer >= waves[currentWaveCount].spawnInterfal)
			{
				spawnTimer = 0;
				SpawnEnemies();
			}
		}

		private void BaginNextWave()
		{

			if (currentWaveCount < waves.Count - 1)
			{
				waves[currentWaveCount].spawnCount = 0;
				currentWaveCount++;
			}
			else
			{
				currentWaveCount = 0;
				waves[currentWaveCount].spawnCount = 0;
			}

			if (currentWaveCount >= waves.Count - 1)
			{
				StartCoroutine(AlertBigBoss());
			}
		}

		private IEnumerator AlertBigBoss()
		{
			alert.SetActive(true);
			yield return new WaitForSeconds(5.0f);
			alert.SetActive(false);
		}

		private void CalculateWaveQuota()
		{
			foreach (var wave in waves)
			{
				int currentWaveQuota = 0;
				foreach (EnemyGroup enemyGroup in wave.enemyGroups)
				{
					currentWaveQuota += enemyGroup.enemyCount;
				}
				wave.waveQuota = currentWaveQuota;
			}
		}

		private void SpawnEnemies()
		{
			if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
			{
				foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
				{
					if (enemyGroup.spawnCount < enemyGroup.enemyCount)
					{
						Vector2 spawnPosition = new Vector2(player.position.x + Random.Range(-30, 30), player.position.y + Random.Range(-30, 30));
						ObjectPoolManager.SpawnObject(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.Enemy);
						enemyGroup.spawnCount++;
						waves[currentWaveCount].spawnCount++;
					}
				}
			}
		}
	}
}
