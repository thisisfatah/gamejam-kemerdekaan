using RadioRevolt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadioRevolt
{

	public class MapController : MonoBehaviour
	{
		[SerializeField] private List<GameObject> terrainChunks;
		[SerializeField] private GameObject player;

		[SerializeField] private float checkerRadius;

		[SerializeField] private LayerMask terrainMask;

		[HideInInspector] public GameObject currentChunk;

		private Vector3 noTerrainPosition;

		private PlayerMovement playerMovement;

		private void Start()
		{
			playerMovement = FindObjectOfType<PlayerMovement>();
		}

		private void Update()
		{
			ChunkChecker();
		}

		private void ChunkChecker()
		{
			if (!currentChunk || playerMovement.moveDir == Vector2.zero) return;

			if (playerMovement.moveDir.x > 0 && playerMovement.moveDir.y == 0) // right
			{
				if (CheckOverlap("Right"))
				{
					noTerrainPosition = currentChunk.transform.Find("Right").position;
					SpawnChunk();
				}
			}
			else if (playerMovement.moveDir.x < 0 && playerMovement.moveDir.y == 0) // left
			{
				if (CheckOverlap("Left"))
				{
					noTerrainPosition = currentChunk.transform.Find("Left").position;
					SpawnChunk();
				}
			}
			else if (playerMovement.moveDir.x == 0 && playerMovement.moveDir.y > 0) // up
			{
				if (CheckOverlap("Up"))
				{
					noTerrainPosition = currentChunk.transform.Find("Up").position;
					SpawnChunk();
				}
			}
			else if (playerMovement.moveDir.x == 0 && playerMovement.moveDir.y < 0) // down
			{
				if (CheckOverlap("Down"))
				{
					noTerrainPosition = currentChunk.transform.Find("Down").position;
					SpawnChunk();
				}
			}
			else if (playerMovement.moveDir.x > 0 && playerMovement.moveDir.y > 0) // right up
			{
				if (CheckOverlap("RightUp"))
				{
					noTerrainPosition = currentChunk.transform.Find("RightUp").position;
					SpawnChunk();
				}
			}
			else if (playerMovement.moveDir.x > 0 && playerMovement.moveDir.y < 0) // right down
			{
				if (CheckOverlap("RightDown"))
				{
					noTerrainPosition = currentChunk.transform.Find("RightDown").position;
					SpawnChunk();
				}
			}
			else if (playerMovement.moveDir.x < 0 && playerMovement.moveDir.y > 0) // left up
			{
				if (CheckOverlap("LeftUp"))
				{
					noTerrainPosition = currentChunk.transform.Find("LeftUp").position;
					SpawnChunk();
				}
			}
			else if (playerMovement.moveDir.x < 0 && playerMovement.moveDir.y < 0) // left down
			{
				if (CheckOverlap("LeftDown"))
				{
					noTerrainPosition = currentChunk.transform.Find("LeftDown").position;
					SpawnChunk();
				}
			}
		}

		private void SpawnChunk()
		{
			int rand = Random.Range(0, terrainChunks.Count);
			GameObject chunk = ObjectPoolManager.SpawnObject(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
			chunk.transform.SetParent(transform);
		}

		private bool CheckOverlap(string objName)
		{
			return !Physics2D.OverlapCircle(currentChunk.transform.Find(objName).position, checkerRadius, terrainMask);
		}
	}

}