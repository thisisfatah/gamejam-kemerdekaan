using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float range;
	[SerializeField] private float maxDistance;

	[SerializeField] private bool isBoss;

	[SerializeField] private int health = 10;

	PlayerManager playerManager;
	public bool IsFacingRight { get; private set; }

	private Vector2 wayPoint;

	private void Start()
	{
		SetNewDestination();
		playerManager = FindObjectOfType<PlayerManager>();
		IsFacingRight = true;
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
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}

	private void SetNewDestination()
	{
		wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
	}

	public void GetDamage()
	{
		health--;
		if(health <= 0)
		{
			int score = PlayerPrefs.GetInt("Score");
			PlayerPrefs.SetInt("Score", score + 3);
			Destroy(gameObject);
		}
	}
}
