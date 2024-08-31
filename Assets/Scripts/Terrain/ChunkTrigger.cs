using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
	private MapController controller;
	[SerializeField] GameObject targetMap;

	 public bool playerInArea;

	private void Start()
	{
		controller = FindObjectOfType<MapController>();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			controller.currentChunk = targetMap;
			playerInArea = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (controller.currentChunk == targetMap)
			{
				controller.currentChunk = null;
				playerInArea = false;
			}
		}
	}
}
