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
		[SerializeField] List<GameObject> spawnGameObject;

		private void Start()
		{
			SpawnPlayer();
		}

		public void SpawnPlayer()
		{
			for (int i = 0; i < spawnGameObject.Count; i++)
			{
				Vector3 offsetSpawn = new Vector3(Random.Range(-5,5), Random.Range(-5,5));
				GameObject objectSpawn = ObjectPoolManager.SpawnObject(spawnGameObject[i], spawnTransform.position + offsetSpawn, Quaternion.identity, ObjectPoolManager.PoolType.Player);
			}
		}
	}
}
