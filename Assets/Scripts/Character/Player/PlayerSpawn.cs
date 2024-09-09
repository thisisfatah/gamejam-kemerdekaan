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

		private void Start()
		{
			SpawnPlayer();
		}

		public void SpawnPlayer()
		{
			GameObject objectSpawn = ObjectPoolManager.SpawnObject(spawnGameObject, spawnTransform.position, Quaternion.identity, ObjectPoolManager.PoolType.Player);
			/*PlayerGateManager playerGateManager = objectSpawn.GetComponent<PlayerGateManager>();

			playerGateManager.Init();*/
		}
	}
}
