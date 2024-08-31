using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[System.Serializable]
	public class Sound
	{
		public string name;

		public AudioClip sound;

		public float volume;

		public bool loop;

		[HideInInspector]
		public AudioSource source;
	}

	public static AudioManager Instance;

	[SerializeField] Sound[] sounds;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.sound;
			s.source.volume = s.volume;
			s.source.loop = s.loop;
		}
	}

	public void PlaySound(string soundName)
	{
		if (Instance == null) return;
		if (sounds.Length == 0) return;

		Sound sound = Array.Find(sounds, s => s.name == soundName);

		if (sound == null && sound.sound == null) return;

		sound.source.Play();
	}

	public void StopSound(string soundName)
	{
		if (Instance == null) return;

		Sound sound = Array.Find(sounds, s => s.name == soundName);

		if (sound == null) return;

		sound.source.Stop();
	}

	public void MuteSound(bool isMute)
	{
		foreach (Sound s in sounds)
		{
			s.source.mute = isMute;
		}
	}
}
