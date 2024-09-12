using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RadioRevolt
{
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float _moveSpeed = 10.0f;

		public Rigidbody2D _rigid { get; private set; }

		private PlayerManager _manager;

		[HideInInspector] public Vector2 moveDir { get; private set; }

		public bool IsFacingRight { get; private set; }

		private bool IsSoundPlay;


		private void Awake()
		{
			_rigid = GetComponent<Rigidbody2D>();
			_manager = GetComponent<PlayerManager>();

			IsFacingRight = true;
		}

		private void FixedUpdate()
		{
			_rigid.AddForce(_moveSpeed * moveDir);

			for (int i = 0; i < transform.childCount; i++)
			{
				if (moveDir.x != 0)
					CheckDirectionToFace(moveDir.x > 0);
			}
			PlayAudio();
		}

		private void PlayAudio()
		{
			if (Mathf.Abs(_rigid.velocity.magnitude) > 0.5f && !IsSoundPlay)
			{
				IsSoundPlay = true;
				AudioManager.Instance.PlaySound("FootStep");
			}
			else if (Mathf.Abs(_rigid.velocity.magnitude) < 0.5f && IsSoundPlay)
			{
				IsSoundPlay = false;
				AudioManager.Instance.StopSound("FootStep");
			}
		}

		void OnMove(InputValue value)
		{
			moveDir = value.Get<Vector2>().normalized;
		}

		public void CheckDirectionToFace(bool isMovingRight)
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
	}
}
