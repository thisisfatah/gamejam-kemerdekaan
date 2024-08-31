using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _moveSpeed = 10.0f;

	public Rigidbody2D _rigid { get; private set; }

	private PlayerManager _manager;

	[HideInInspector] public Vector2 moveDir;

	public bool IsFacingRight { get; private set; }

	private void Awake()
	{
		_rigid = GetComponent<Rigidbody2D>();
		_manager = GetComponent<PlayerManager>();

		IsFacingRight = true;
	}

	private void FixedUpdate()
	{
        _rigid.AddForce(_moveSpeed * moveDir);

		if (moveDir.x != 0)
			CheckDirectionToFace(moveDir.x > 0);
	}

	void OnMove(InputValue value)
	{
		moveDir = value.Get<Vector2>().normalized;
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
}
