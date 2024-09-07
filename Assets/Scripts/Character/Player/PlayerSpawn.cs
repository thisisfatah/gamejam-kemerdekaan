using DG.Tweening;
using RadioRevolt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadioRevolt
{
	public class PlayerSpawn : MonoBehaviour
	{
		[SerializeField] Transform spawnTransform;
		[SerializeField] GameObject spawnGameObject;

		[Range(0f, 1f)]
		[SerializeField] private float _distanceFactor;
		[Range(0f, 1f)]
		[SerializeField] private float _radius;

		[SerializeField] private float spawnTimeInMinutes;

		private void Start()
		{
			float spawnTime = spawnTimeInMinutes * 60f;
			InvokeRepeating("SpawnPlayer", 0.1f, spawnTime);
		}

		public void SpawnPlayer()
		{
			GameObject objectSpawn = ObjectPoolManager.SpawnObject(spawnGameObject, spawnTransform.position, Quaternion.identity, ObjectPoolManager.PoolType.Player);
			PlayerGateManager playerGateManager = objectSpawn.GetComponent<PlayerGateManager>();

			playerGateManager.Init();
			FormatCharacter();
		}

		protected void FormatCharacter()
		{
			for (int i = 0; i < spawnTransform.childCount; i++)
			{
				float x = _distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _radius);
				float y = _distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _radius);

				Vector2 newPos = new Vector2(x, y);
				spawnTransform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
			}
		}
	}
}
