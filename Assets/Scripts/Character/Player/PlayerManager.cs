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

		[HideInInspector] public bool attack;

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
			FormatCharacter();
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
				enemyTransform = collision.transform;
				attack = true;
			}

			if (collision.CompareTag("Gate"))
			{
				collision.gameObject.tag = "Untagged";

				SpawnPlayer(collision.GetComponent<Player>());
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
