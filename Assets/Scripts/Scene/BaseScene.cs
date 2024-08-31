using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class BaseScene : MonoBehaviour
{
	[SerializeField]
	protected Canvas canvas;

	protected Stack<GameObject> currentPopups = new Stack<GameObject>();
	protected Stack<GameObject> currentPanels = new Stack<GameObject>();

	/// <typeparam name="T">The type of the popup.</typeparam>
	public void OpenPopup<T>(string popupName, Action<T> onOpened = null, bool darkenBackground = true) where T : Popup
	{
		StartCoroutine(OpenPopupAsync(popupName, onOpened, darkenBackground));
	}

	public void CloseCurrentPopup()
	{
		GameObject currentPopup = currentPopups.Peek();
		if (currentPopup != null)
		{
			currentPopup.GetComponent<Popup>().Close();
		}
	}


	public void ClosePopup()
	{
		GameObject topmostPopup = currentPopups.Pop();
		if (topmostPopup == null)
		{
			return;
		}

		GameObject topmostPanel = currentPanels.Pop();
		if (topmostPanel != null)
		{
			StartCoroutine(FadeOut(topmostPanel.GetComponent<Image>(), 0.2f, () => Destroy(topmostPanel)));
		}
	}


	protected IEnumerator OpenPopupAsync<T>(string popupName, Action<T> onOpened, bool darkenBackground) where T : Popup
	{
		ResourceRequest request = Resources.LoadAsync<GameObject>(popupName);
		while (!request.isDone)
		{
			yield return null;
		}

		GameObject panel = new GameObject("Panel");
		Image panelImage = panel.AddComponent<Image>();
		Color color = Color.black;
		color.a = 0;
		panelImage.color = color;
		RectTransform panelTransform = panel.GetComponent<RectTransform>();
		panelTransform.anchorMin = new Vector2(0, 0);
		panelTransform.anchorMax = new Vector2(1, 1);
		panelTransform.pivot = new Vector2(0.5f, 0.5f);
		panel.transform.SetParent(canvas.transform, false);
		currentPanels.Push(panel);
		StartCoroutine(FadeIn(panel.GetComponent<Image>(), 0.2f));

		GameObject popup = Instantiate(request.asset) as GameObject;
		Assert.IsNotNull((popup));
		popup.transform.SetParent(canvas.transform, false);
		popup.GetComponent<Popup>().parentScene = this;

		if (onOpened != null)
		{
			onOpened(popup.GetComponent<T>());
		}
		currentPopups.Push(popup);
	}

	protected IEnumerator FadeIn(Image image, float time)
	{
		var alpha = image.color.a;
		for (var t = 0.0f; t < 0.5f; t += Time.deltaTime / time)
		{
			var color = image.color;
			color.a = Mathf.Lerp(alpha, 220 / 256.0f, t);
			image.color = color;
			yield return null;
		}
	}

	protected IEnumerator FadeOut(Image image, float time, Action onComplete)
	{
		float alpha = image.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
		{
			Color color = image.color;
			color.a = Mathf.Lerp(alpha, 0, t);
			image.color = color;
			yield return null;
		}
		if (onComplete != null)
		{
			onComplete();
		}
	}
}
