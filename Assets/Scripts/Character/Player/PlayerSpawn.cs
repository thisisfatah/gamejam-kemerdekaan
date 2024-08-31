using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] Transform spawnTransform;
    [SerializeField] PlayerGateManager spawnGameObject;

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
        PlayerGateManager playerGateManager = Instantiate(spawnGameObject, spawnTransform.position, Quaternion.identity);
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
