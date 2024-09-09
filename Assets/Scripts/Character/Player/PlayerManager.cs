using Cinemachine;
using DG.Tweening;
using RadioRevolt.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RadioRevolt
{
	public class PlayerManager : MonoBehaviour
	{
		private Transform playerTransform;
		private Transform enemyTransform;
		public CinemachineVirtualCamera virtualCamera;

		[HideInInspector] public bool attack;

		[SerializeField] protected GameObject character;
		[Range(0f, 1f)]
		[SerializeField] private float _distanceFactor;
		[Range(0f, 1f)]
		[SerializeField] private float _radius;

		[SerializeField] private CircleCollider2D circleCollider;

		private int numberOfCharacter;

		protected virtual void Start()
		{
			numberOfCharacter = transform.childCount;

			playerTransform = transform;
			virtualCamera.m_Lens.OrthographicSize = 5;
		}

		private void Update()
		{
			if (attack && transform.childCount > 1)
			{
				Vector2 enemyDir = (Vector2)(enemyTransform.position - transform.position);
				if (enemyTransform != null)
				{
					for (int i = 1; i < transform.childCount; i++)
					{
						Vector2 distance = enemyTransform.position - transform.GetChild(i).position;
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
					ObjectPoolManager.ReturnObjectToPool(enemyTransform.gameObject, ObjectPoolManager.PoolType.Enemy);
					attack = false;
					FormatCharacter();
				}
			}
		}

		protected void SpawnPlayer()
		{
			GameObject newPlayer = ObjectPoolManager.SpawnObject(character, transform);

			numberOfCharacter = transform.childCount;

			FormatCharacter();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Gate"))
			{
				PlayerData playerData = collision.GetComponent<PlayerGateManager>().playerData;
				character.GetComponent<Player>().anim.runtimeAnimatorController = playerData.animator;
				character.transform.GetChild(0).GetComponent<Light2D>().enabled = playerData.useLight;

				SpawnPlayer();
				ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.Player);
				virtualCamera.m_Lens.OrthographicSize += 0.1f;
			}

			if (collision.CompareTag("Enemy"))
			{
				enemyTransform = collision.transform;
				attack = true;
				//collision.GetComponent<EnemyManager>().Attack(transform);
			}
		}

		protected void FormatCharacter()
		{
			float yPos = 0;
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).gameObject.activeInHierarchy)
				{
					float x = _distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _radius);
					float y = _distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _radius);

					Vector2 newPos = new Vector2(x, y);
					transform.GetChild(i).DOLocalMove(newPos, 0.1f).SetEase(Ease.OutBack);

					if (i == 0) continue;

					if (y > yPos)
					{
						yPos = y;
					}
				}
			}
			circleCollider.radius = yPos;
		}
	}
}
