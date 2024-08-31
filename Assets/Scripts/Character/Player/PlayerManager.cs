using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

public class PlayerManager : Character
{
	private Transform playerTransform;
	private Transform enemyTransform;
	public CinemachineVirtualCamera virtualCamera;

	[HideInInspector] public bool attack;

	private void Start()
	{
		playerTransform = transform;
		virtualCamera.m_Lens.OrthographicSize = 5;
	}

	private void Update()
	{
		if (attack && transform.childCount > 1)
		{
			Vector2 enemyDir = (Vector2)(enemyTransform.position - transform.position);
			if (enemyTransform.childCount > 0)
			{
				for (int i = 1; i < transform.childCount; i++)
				{
					Vector2 distance = enemyTransform.GetChild(0).position - transform.GetChild(i).position;
					if (distance.magnitude < 3f)
					{
						transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
							enemyTransform.GetChild(0).position,
							Time.deltaTime * 1f);
					}
				}
			}
			else 
			{
				attack = false;
				FormatCharacter();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Gate"))
		{
			PlayerData playerData = collision.GetComponent<PlayerGateManager>().playerData;
			character.GetComponent<Player>().anim.runtimeAnimatorController = playerData.animator;
			character.transform.GetChild(0).GetComponent<Light2D>().enabled = playerData.useLight;
			MakeStickMan(1);
			Destroy(collision.gameObject);
			virtualCamera.m_Lens.OrthographicSize += 0.1f;
		}

		if (collision.CompareTag("Enemy"))
		{
			enemyTransform = collision.transform;
			attack = true;
			collision.GetComponent<EnemyManager>().Attack(transform);
		}
	}
}
