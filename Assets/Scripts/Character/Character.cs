using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
	[SerializeField] protected GameObject character;
	[Range(0f, 1f)]
	[SerializeField] private float _distanceFactor;
	[Range(0f, 1f)]
	[SerializeField] private float _radius;

	[SerializeField] private CircleCollider2D circleCollider;

	private int numberOfCharacter;

	private void Start()
	{
		numberOfCharacter = transform.childCount;
	}

	protected void MakeStickMan(int number)
	{
		for (int i = 0; i < number; i++)
		{
			Instantiate(character, transform.position, Quaternion.identity, transform);
		}

		numberOfCharacter = transform.childCount;

		FormatCharacter();
	}

	protected void FormatCharacter()
	{
		float yPos = 0;
		for (int i = 0; i < transform.childCount; i++)
		{
			float x = _distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * _radius);
			float y = _distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * _radius);

			Vector2 newPos = new Vector2(x, y);
			transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);

			if (i == 0) continue;

			if (y > yPos)
			{
				yPos = y;
			}
		}
		circleCollider.radius = yPos;
	}
}
