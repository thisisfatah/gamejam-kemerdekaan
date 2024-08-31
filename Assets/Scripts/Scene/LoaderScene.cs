using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderScene : BaseScene
{
	[SerializeField] private float delayInSeconds = 20;

	[SerializeField] private List<GameObject> scripts = new List<GameObject>();
	[SerializeField] private List<float> animations = new List<float>();
	private void Start()
	{
		AudioManager.Instance.PlaySound("Broadcast");
		StartCoroutine(DelayLoadScene());
		StartCoroutine(PlayScript());
	}

	private IEnumerator DelayLoadScene()
	{
		yield return new WaitForSeconds(delayInSeconds);

		Transition.LoadLevel("MenuScene", 1.0f, Color.black);
	}

	private IEnumerator PlayScript()
	{
		scripts[0].SetActive(true);
		yield return new WaitForSeconds(animations[0]);
		scripts[0].SetActive(false);
		scripts[1].SetActive(true);
		yield return new WaitForSeconds(animations[1]);
		scripts[1].SetActive(false);
		scripts[2].SetActive(true);
	}
}
