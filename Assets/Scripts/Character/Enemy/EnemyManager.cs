using RadioRevolt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadioRevolt
{
	public class EnemyManager : SpawnCharacterRadioRevolt
	{
		private bool attack;
		private Transform enemyTransform;

		protected override void Start()
		{
			base.Start();

			MakeStickMan(20);
		}

		private void Update()
		{
			if (attack && transform.childCount > 0)
			{
				Vector2 enemyDir = enemyTransform.position - transform.position;

				for (int i = 0; i < transform.childCount; i++)
				{
					Vector2 distance = enemyTransform.GetChild(0).position - transform.GetChild(i).position;

					if (distance.magnitude < 3f)
					{
						transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
								enemyTransform.GetChild(0).position,
								Time.deltaTime * 2f);
					}
				}
			}
			else if(attack && transform.childCount <= 0)
			{
				attack = false;
				ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Enemy);
			}
		}

		public void Attack(Transform enemyForce)
		{
			attack = true;
			enemyTransform = enemyForce;
		}

		private void OnEnable()
		{
			MakeStickMan(20);
		}
	}
}