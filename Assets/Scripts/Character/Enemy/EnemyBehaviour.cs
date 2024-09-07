using RadioRevolt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadioRevolt
{
	public class EnemyBehaviour : CharacterRadioRevolt
	{
		[SerializeField] private float speed;
		[SerializeField] private float range;
		[SerializeField] private float maxDistance;

		[SerializeField] private bool isBoss;

		PlayerManager playerManager;
		public bool IsFacingRight { get; private set; }

		private Vector2 wayPoint;

		private void Start()
		{
			SetNewDestination();
			playerManager = FindObjectOfType<PlayerManager>();
			IsFacingRight = true;

			OnDieEvent.AddListener(() => KilledEnemy());
		}

		private void Update()
		{
			Vector2 moveDir = Vector2.zero;
			if (isBoss)
			{
				moveDir = (playerManager.transform.position - transform.position).normalized;
				transform.position = Vector2.MoveTowards(transform.position, playerManager.transform.position, speed * Time.deltaTime);

			}
			else
			{
				moveDir = (wayPoint - (Vector2)transform.position).normalized;
				transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
				if (Vector2.Distance(transform.position, wayPoint) < range)
				{
					SetNewDestination();
				}
			}

			if (moveDir.x != 0)
				CheckDirectionToFace(moveDir.x > 0);
		}

		private void CheckDirectionToFace(bool isMovingRight)
		{
			if (isMovingRight != IsFacingRight)
				Turn();
		}

		private void Turn()
		{
			if (transform.childCount > 0)
			{
				for (int i = 0; i < transform.childCount; i++)
				{
					Vector3 scale = transform.GetChild(i).localScale;
					scale.x *= -1;
					transform.GetChild(i).localScale = scale;
				}
				IsFacingRight = !IsFacingRight;
			}
		}

		private void SetNewDestination()
		{
			wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
		}

		private void KilledEnemy()
		{
			int score = PlayerPrefs.GetInt("Score");
			PlayerPrefs.SetInt("Score", score + 3);
			ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Enemy);
			FullHealth();
		}
	}

}