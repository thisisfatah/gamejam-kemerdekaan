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

	public class Player : MonoBehaviour
	{
		public Animator anim;
		private PlayerMovement playerMovement;
		private PlayerManager playerManager;

		[SerializeField] private int health;

		[SerializeField] private PlayerType playerType;

		[SerializeField] private GameScene gameScene;

		public bool isGameOver = false;

		public bool IsFacingRight { get; private set; }

		private void Awake()
		{
			if(transform.parent != null)
			{
				playerManager = transform.parent.GetComponent<PlayerManager>();
				playerMovement = playerManager.GetComponent<PlayerMovement>();
			}

			gameScene = FindObjectOfType<GameScene>();
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
			if(playerManager == null) return;

			/*if (playerType == PlayerType.Main)
			{
				if (collision.CompareTag("Red") && collision.transform.parent.childCount > 0)
				{
					int score = PlayerPrefs.GetInt("Score");
					PlayerPrefs.SetInt("Score", score + 1);
					GetDamage();
					ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.Enemy);
				}
				else if (collision.CompareTag("MiniBoss"))
				{
					EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
					enemyBehaviour.IncreaseHealth(1);
					GetDamage();
				}
				else if (collision.CompareTag("Boss"))
				{
					EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
					enemyBehaviour.IncreaseHealth(1);
					GetDamage();
				}
			}
			else
			{
				if (collision.CompareTag("Red") && collision.transform.parent.childCount > 0)
				{
					ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.Enemy);
					ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
					int score = PlayerPrefs.GetInt("Score");
					PlayerPrefs.SetInt("Score", score + 1);
					player.virtualCamera.m_Lens.OrthographicSize -= 0.05f;
				}
				else if (collision.CompareTag("MiniBoss"))
				{
					EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
					enemyBehaviour.IncreaseHealth(1);
					ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
				}
				else if (collision.CompareTag("Boss"))
				{
					EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
					enemyBehaviour.IncreaseHealth(1);
					ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
				}
			}*/

			if (collision.CompareTag("Enemy"))
			{
				int score = PlayerPrefs.GetInt("Score");
				PlayerPrefs.SetInt("Score", score + 1);

				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.IncreaseHealth(1);

				GetDamage();

				//Destroy(collision.gameObject);
				//ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.Enemy);
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (playerType == PlayerType.Main)
			{
				if (collision.CompareTag("MiniBoss"))
				{
					EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
					enemyBehaviour.IncreaseHealth(1);
					GetDamage();
				}
				else if (collision.CompareTag("Boss"))
				{
					EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
					enemyBehaviour.IncreaseHealth(1);
					GetDamage();
				}
			}
		}

		private void GetDamage()
		{
			health--;

			if (!isGameOver)
			{
				if (health <= 0)
				{
					isGameOver = true;
					ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.Player);
					switch (playerType)
					{
						case PlayerType.Main:
							gameScene.OpenPopup<GameOverPopUp>("Popups/GameOverPopup");
							break;
					}
				}
			}
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

		private void OnEnable()
		{
			IsFacingRight = true;
		}
	}
}