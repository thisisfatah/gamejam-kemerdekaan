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
			if (attack && enemyTransform != null)
			{
				Vector2 enemyDir = (Vector2)(enemyTransform.position - transform.position);

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
				attack = false;
				FormatCharacter();
			}
		}

		protected void SpawnPlayer(Player player)
		{
			player.transform.SetParent(transform);
			player.transform.position = transform.position;
			player.transform.rotation = transform.rotation;
			player.transform.localScale = Vector3.one;

			player.Init();

			numberOfCharacter = transform.childCount;

			FormatCharacter();
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Enemy"))
			{
				Debug.Log("Test");

				enemyTransform = collision.transform;
				attack = true;
				//collision.GetComponent<EnemyManager>().Attack(transform);
			}

			if (collision.CompareTag("Gate"))
			{
				collision.gameObject.tag = "Untagged";

				SpawnPlayer(collision.GetComponent<Player>());
				//ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.Player);
				virtualCamera.m_Lens.OrthographicSize += 0.1f;
			}

		}

		protected void FormatCharacter()
		{
			float yPos = 0;
			for (int i = 0; i < transform.childCount; i++)
			{
				float x = _distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _radius);
				float y = _distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _radius);

				Vector2 newPos = new Vector2(x, y);
				transform.GetChild(i).DOLocalMove(newPos, 0.5f).SetEase(Ease.OutBack);

				if (i == 0) continue;

				if (y > yPos)
				{
					yPos = y;
				}
			}
			circleCollider.radius = yPos;
		}
	}
}
