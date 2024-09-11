using RadioRevolt.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RadioRevolt
{
	public enum PlayerType
	{
		Main,
		Side
	}

	public class Player : CharacterRadioRevolt
	{
		public Animator anim;
		private PlayerMovement playerMovement;
		private PlayerManager playerManager;


		[SerializeField] private PlayerType playerType;

		private GameScene gameScene;

		public bool isGameOver = false;

		public bool IsFacingRight { get; private set; }

		private void Awake()
		{
			if (transform.parent != null)
			{
				playerManager = transform.parent.GetComponent<PlayerManager>();
				if (playerManager != null)
				{
					playerMovement = playerManager.GetComponent<PlayerMovement>();
				}
			}

			gameScene = FindObjectOfType<GameScene>();

			OnDieEvent.AddListener(OnKilled);
		}

		private void LateUpdate()
		{
			if (playerMovement == null) return;

			anim.SetBool("IsRun", playerMovement.moveDir != Vector2.zero);
		}

		public void Init()
		{
			if (transform.parent != null)
			{
				playerManager = transform.parent.GetComponent<PlayerManager>();
				playerMovement = playerManager.GetComponent<PlayerMovement>();
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (playerManager == null) return;

			if (collision.CompareTag("Enemy"))
			{
				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.IncreaseHealth(1);

				IncreaseHealth(1);
			}
		}

		private void OnKilled()
		{
			if (!gameScene.IsGameOver && playerType == PlayerType.Main)
			{
				gameScene.EndGame();
			}
            else
            {
				ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
				ResetPlayer();
			}

		}

		private void ResetPlayer()
		{
			playerManager = null;
			playerMovement = null;
			gameObject.tag = "Gate";
		}

		private void OnEnable()
		{
			IsFacingRight = true;
		}
	}
}