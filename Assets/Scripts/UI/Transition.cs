using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
	private static GameObject canvasObject;

	private GameObject _overlay;

	private void Awake()
	{
		canvasObject = new GameObject("TransitionCanvas");
		Canvas canvas = canvasObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 99;
		DontDestroyOnLoad(canvasObject);
	}

	public static void LoadLevel(string level, float duration, Color fadeColor)
	{
		GameObject fade = new GameObject("Transition");
		fade.AddComponent<Transition>();
		fade.GetComponent<Transition>().StartFade(level, duration, fadeColor);
		fade.transform.SetParent(canvasObject.transform, false);
		fade.transform.SetAsLastSibling();
	}

	private void StartFade(string level, float duration, Color fadeColor)
	{
		StartCoroutine(RunFade(level, duration, fadeColor));
	}

	private IEnumerator RunFade(string level, float duration, Color fadeColor)
	{
		Texture2D bgTex = new Texture2D(1, 1);
		bgTex.SetPixel(0, 0, fadeColor);
		bgTex.Apply();

		_overlay = new GameObject();
		Image image = _overlay.AddComponent<Image>();
		Rect rect = new Rect(0, 0, bgTex.width, bgTex.height);
		Sprite sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
		image.material.mainTexture = bgTex;
		image.sprite = sprite;
		Color newColor = image.color;
		image.color = newColor;
		image.canvasRenderer.SetAlpha(0.0f);

		_overlay.transform.localScale = new Vector3(1, 1, 1);
		_overlay.GetComponent<RectTransform>().sizeDelta = canvasObject.GetComponent<RectTransform>().sizeDelta;
		_overlay.transform.SetParent(canvasObject.transform, false);
		_overlay.transform.SetAsFirstSibling();

		float time = 0.0f;
		float halfDuration = duration / 2.0f;
		while (time < halfDuration)
		{
			time += Time.deltaTime;
			image.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
			yield return new WaitForEndOfFrame();
		}

		image.canvasRenderer.SetAlpha(1.0f);
		yield return new WaitForEndOfFrame();

		SceneManager.LoadScene(level);

		time = 0.0f;
		while (time < halfDuration)
		{
			time += Time.deltaTime;
			image.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / halfDuration));
			yield return new WaitForEndOfFrame();
		}

		image.canvasRenderer.SetAlpha(0.0f);
		yield return new WaitForEndOfFrame();

		Destroy(canvasObject);
	}
}
