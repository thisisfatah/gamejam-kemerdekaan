using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Character
{
	private bool attack;
	private Transform enemyTransform;

	private void Start()
	{
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

				if(distance.magnitude < 3f)
				{
					transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
							enemyTransform.GetChild(0).position,
							Time.deltaTime * 2f);
				}
			}
		}
	}

	public void Attack(Transform enemyForce)
	{
		attack = true;
		enemyTransform = enemyForce;
	}
}
