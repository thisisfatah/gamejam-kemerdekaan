using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RadioRevolt.Utils
{
	public class ObjectPoolManager : MonoBehaviour
	{
		public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

		private GameObject _objectPoolEmptyHolder;

		private static GameObject _enemyObjectsEmpty;
		private static GameObject _playerObjectsEmpty;

		public enum PoolType
		{
			Enemy,
			Player,
			None
		}

		private void Awake()
		{
			SetupEmpties();
		}

		private void SetupEmpties()
		{
			_objectPoolEmptyHolder = new GameObject("Pooled Objects");

			_enemyObjectsEmpty = new GameObject("Enemy");
			_enemyObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);

			_playerObjectsEmpty = new GameObject("Player");
			_playerObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
		}

		public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
		{
			PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

			if (pool == null)
			{
				pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
				ObjectPools.Add(pool);
			}

			GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

			if (spawnableObj == null)
			{
				GameObject parentObj = SetParentObject(poolType);
				spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

				if (parentObj != null)
				{
					spawnableObj.transform.SetParent(parentObj.transform);
				}
			}
			else
			{
				spawnableObj.transform.position = spawnPosition;
				spawnableObj.transform.rotation = spawnRotation;
				spawnableObj.transform.localScale = Vector3.one;
				pool.InactiveObjects.Remove(spawnableObj);
				spawnableObj.SetActive(true);
			}

			return spawnableObj;
		}

		public static GameObject SpawnObject(GameObject objectToSpawn, Transform transform)
		{
			PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

			if (pool == null)
			{
				pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
				ObjectPools.Add(pool);
			}

			GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

			if (spawnableObj == null)
			{
				spawnableObj = Instantiate(objectToSpawn, transform);
				spawnableObj.transform.position = transform.position;
				spawnableObj.transform.localScale = new Vector3(1, 1, 1);
			}
			else
			{
				pool.InactiveObjects.Remove(spawnableObj);
				spawnableObj.SetActive(true);
				spawnableObj.transform.SetParent(transform);
				spawnableObj.transform.position = transform.position;
				spawnableObj.transform.localScale = new Vector3(1, 1, 1);
			}

			return spawnableObj;
		}

		public static void ReturnObjectToPool(GameObject obj, PoolType poolType)
		{
			string objName = obj.name.Substring(0, obj.name.Length - 7);

			PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objName);

			if (pool == null)
			{
				Debug.LogWarning("Trying to release an object that is not pooled: " + obj.name);
			}
			else
			{
				GameObject parentObj = SetParentObject(poolType);
				if (parentObj != null)
				{
					obj.transform.SetParent(parentObj.transform);
				}

				obj.transform.position = parentObj.transform.position;
				obj.transform.rotation = Quaternion.identity;
				obj.transform.localScale = Vector3.one;

				obj.SetActive(false);
				pool.InactiveObjects.Add(obj);


			}
		}

		private static GameObject SetParentObject(PoolType poolType)
		{
			switch (poolType)
			{
				case PoolType.Enemy:
					return _enemyObjectsEmpty;
				case PoolType.Player:
					return _playerObjectsEmpty;
				case PoolType.None:
					return null;
				default:
					return null;
			}
		}
	}

	[System.Serializable]
	public class PooledObjectInfo
	{
		public string LookupString;
		public List<GameObject> InactiveObjects = new List<GameObject>();
	}

}

