using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

	private void Awake()
	{
		if(AudioManager.Instance == null)
		{
			Instantiate(audioManager);
		}
	}
}
