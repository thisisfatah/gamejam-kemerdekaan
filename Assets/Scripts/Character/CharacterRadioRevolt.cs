using RadioRevolt.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RadioRevolt
{
	public class CharacterRadioRevolt : MonoBehaviour
	{
		[SerializeField] private float health;
		[SerializeField] private float maxhealth;
		[SerializeField] private float durationFlash;

		[SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private Material flashMaterial;

		private Material originalMaterial;
		private Coroutine flashRoutine;

		public float Health { get { return health; } set { health = value; } }
		public float MaxHealth { get { return maxhealth; } private set { maxhealth = value; } }

		public UnityEvent OnDieEvent;

		private void Start()
		{
		}

		public virtual void IncreaseHealth(float value)
		{
			if (health <= 0) return;

			Flash();
			health -= value;

			Debug.Log(health + " " + gameObject.name);
		}

		public virtual void DecreaseHealth(float value)
		{
			if (health <= maxhealth)
			{
				health += value;
			}
		}

		public virtual void FullHealth()
		{
			health = maxhealth;
			spriteRenderer.material = originalMaterial;
		}


		private void Flash()
		{
			if (flashRoutine != null)
			{
				spriteRenderer.material = originalMaterial;
				StopCoroutine(flashRoutine);
			}
			else
			{
				originalMaterial = spriteRenderer.material;
			}

			flashRoutine = StartCoroutine(FlashDelay());
		}

		private IEnumerator FlashDelay()
		{
			spriteRenderer.material = flashMaterial;
			yield return new WaitForSeconds(durationFlash);
			Debug.Log("back to material");
			spriteRenderer.material = originalMaterial;

			if (health <= 0)
			{
				if (OnDieEvent != null)
					OnDieEvent.Invoke();
			}
		}
	}
}
