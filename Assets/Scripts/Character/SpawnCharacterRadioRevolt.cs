using DG.Tweening;
using RadioRevolt.Utils;
using UnityEngine;

namespace RadioRevolt
{
	public class SpawnCharacterRadioRevolt : MonoBehaviour
	{
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
		}

		protected void MakeStickMan(int number)
		{
			for (int i = 0; i < number; i++)
			{
				ObjectPoolManager.SpawnObject(character, transform);
			}

			numberOfCharacter = transform.childCount;

			FormatCharacter();
		}

		protected void FormatCharacter()
		{
			float yPos = 0;
			for (int i = 0; i < transform.childCount; i++)
			{
				if(transform.GetChild(i).gameObject.activeInHierarchy)
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

