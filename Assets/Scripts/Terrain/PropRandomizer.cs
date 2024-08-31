using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
	[SerializeField] private List<GameObject> propPrefabs;

	[SerializeField] private List<GameObject> houseProfab;

	[SerializeField] float spawnAreaWidth = 1.0f;
	[SerializeField] float spawnAreaHeight = 1.0f;

	[SerializeField] float delaySpawnHouse = 10.0f;
	[SerializeField] ChunkTrigger chunkTrigger;

	private GameObject houseObject;

	private Coroutine houseCoroutine;

	private Transform player;

	private void Start()
	{
		SpawnProps();
		player = FindObjectOfType<PlayerManager>().transform;
		houseCoroutine = StartCoroutine(SpawnHouse());
	}

	private void SpawnProps()
	{
		for (int i = 0; i < 5; i++)
		{
			int rand = Random.Range(0, propPrefabs.Count);
			GameObject go = Instantiate(propPrefabs[rand], transform);
			Transform t = go.transform;
			Vector3 pos = transform.position;

			pos.x += Random.Range(-spawnAreaWidth, spawnAreaWidth);
			pos.y += Random.Range(-spawnAreaHeight, spawnAreaHeight);

			t.position = pos;
		}
	}

	private IEnumerator SpawnHouse()
	{
		if (houseProfab.Count == 0) yield return null;

		float randSpawnTime = Random.Range(5, delaySpawnHouse);

		yield return new WaitForSeconds(randSpawnTime / 2);

		int rand = Random.Range(0, houseProfab.Count);

		houseObject = Instantiate(houseProfab[rand], transform);
		Transform t = houseObject.transform;
		Vector3 pos = transform.position;

		pos.x += Random.Range(-spawnAreaWidth, spawnAreaWidth);
		pos.y += Random.Range(-spawnAreaHeight, spawnAreaHeight);

		t.position = pos;

		yield return new WaitForSeconds(randSpawnTime);

		if (houseObject != null)
		{
			Destroy(houseObject);
		}

		houseCoroutine = StartCoroutine(SpawnHouse());
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth * 2, spawnAreaHeight * 2));
	}
}
