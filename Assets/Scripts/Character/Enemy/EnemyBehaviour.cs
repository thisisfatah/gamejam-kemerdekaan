using Pathfinding;
using RadioRevolt.Utils;
using UnityEngine;

namespace RadioRevolt
{
	public class EnemyBehaviour : CharacterRadioRevolt
	{
		[System.Serializable]
		public enum BossType
		{
			Prajurit,
			MiniBoss,
			Boss
		}

		[Header("PathFinding")]
		private Transform target;
		[SerializeField] private float randomDistance = 0.5f;
		[SerializeField] private float playerDistance = 20f;
		[SerializeField] private float pathUpdateSeconds = 0.5f;

		[Header("Physics")]
		[SerializeField] private float speed = 200f;
		private float nextWayPointDistance = 3f;

		[Header("Custom Behaviour")]
		[SerializeField] private bool followEnabled = true;
		[SerializeField] private bool directionLookEnabled = true;
		[SerializeField] private BossType type;

		private PlayerManager playerManager;
		private Path path;
		private Seeker seeker;
		private Rigidbody2D rb;

		private Vector2 wayPoint;
		public bool IsFacingRight { get; private set; }
		private int currentWayPoint = 0;

		private GameScene gameScene;

		[Header("Check")]
		[SerializeField] private float maxDistance;
		[SerializeField] private bool isBoss;

		private void Start()
		{
			seeker = GetComponent<Seeker>();
			rb = GetComponent<Rigidbody2D>();

			playerManager = FindObjectOfType<PlayerManager>();
			gameScene = FindObjectOfType<GameScene>();
			IsFacingRight = true;

			SetNewDistance();
			target = playerManager.transform;

			OnDieEvent.AddListener(() => KilledEnemy());

			InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
		}

		private void FixedUpdate()
		{
			if (followEnabled)
			{
				PathFollow();
			}
		}

		private void UpdatePath()
		{
			if (followEnabled && seeker.IsDone())
			{
				seeker.StartPath(rb.position, Destination(), OnPathComplete);
			}
		}

		private void PathFollow()
		{
			if (path == null)
			{
				return;
			}

			if (currentWayPoint >= path.vectorPath.Count)
			{
				return;
			}

			Vector2 dir = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
			Vector2 force = dir * speed * Time.deltaTime;

			rb.AddForce(force, ForceMode2D.Force);

			float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

			if (distance < nextWayPointDistance)
			{
				currentWayPoint++;
			}

			if (Vector2.Distance(rb.position, wayPoint) < randomDistance)
			{
				SetNewDistance();
			}

			if (directionLookEnabled)
			{
				if (rb.velocity.x != 0)
					CheckDirectionToFace(rb.velocity.x > 0);
			}
		}

		private void OnPathComplete(Path p)
		{
			if (!p.error)
			{
				path = p;
				currentWayPoint = 0;
			}
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
					Vector3 scale = transform.localScale;
					scale.x *= -1;
					transform.localScale = scale;
				}
				IsFacingRight = !IsFacingRight;
			}
		}

		private Vector2 Destination()
		{
			if (Vector2.Distance(rb.position, (Vector2)target.position) < playerDistance)
			{
				return (Vector2)target.position;
			}
			else
			{
				return wayPoint;
			}
		}

		private void SetNewDistance()
		{
			wayPoint = (Vector2)transform.position + new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
		}

		private void KilledEnemy()
		{
			if (!gameScene.IsGameOver && type == BossType.Boss)
			{
				gameScene.EndGame();
			}
			Destroy(gameObject);
			FullHealth();
		}
	}

}