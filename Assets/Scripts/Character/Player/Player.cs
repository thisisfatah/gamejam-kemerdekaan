using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
	Main,
	Side
}

public class Player : MonoBehaviour
{
	public Animator anim;
	private PlayerMovement playerMovement;
	private PlayerManager player;

	[SerializeField] private int health;

	[SerializeField] private PlayerType playerType;

	[SerializeField] private GameScene gameScene;

	public bool isGameOver = false;

	private void Awake()
	{
		playerMovement = transform.parent.GetComponent<PlayerMovement>();
		player = transform.parent.GetComponent<PlayerManager>();
	}

	private void LateUpdate()
	{
		anim.SetBool("IsRun", playerMovement.moveDir != Vector2.zero);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (playerType == PlayerType.Main)
		{
			if (collision.CompareTag("Red") && collision.transform.parent.childCount > 0)
			{
				int score = PlayerPrefs.GetInt("Score");
				PlayerPrefs.SetInt("Score", score + 1);
				GetDamage();
				Destroy(collision.gameObject);
			}
			else if (collision.CompareTag("MiniBoss"))
			{
				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.GetDamage();
				GetDamage();
			}
			else if (collision.CompareTag("Boss"))
			{
				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.GetDamage();
				GetDamage();
			}
		}
		else
		{
			if (collision.CompareTag("Red") && collision.transform.parent.childCount > 0)
			{
				Destroy(collision.gameObject);
				Destroy(gameObject);
				int score = PlayerPrefs.GetInt("Score");
				PlayerPrefs.SetInt("Score", score + 1);
				player.virtualCamera.m_Lens.OrthographicSize -= 0.1f;
			}
			else if (collision.CompareTag("MiniBoss"))
			{
				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.GetDamage();
				Destroy(gameObject);
				player.virtualCamera.m_Lens.OrthographicSize -= 0.1f;
			}
			else if (collision.CompareTag("Boss"))
			{
				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.GetDamage();
				Destroy(gameObject);
				player.virtualCamera.m_Lens.OrthographicSize -= 0.1f;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (playerType == PlayerType.Main)
		{
			if (collision.CompareTag("MiniBoss"))
			{
				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.GetDamage();
				GetDamage();
			}
			else if (collision.CompareTag("Boss"))
			{
				EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
				enemyBehaviour.GetDamage();
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
				gameScene.OpenPopup<GameOverPopUp>("Popups/GameOverPopup");
			}
		}
	}
}
