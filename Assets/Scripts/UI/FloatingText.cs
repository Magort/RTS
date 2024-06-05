using TMPro;
using UnityEngine;

public class FloatingText
{
	public bool isActive;
	public GameObject gObject;
	public TextMeshProUGUI text;
	public Color color;
	public Vector3 motion;
	public float duration;
	public float lastShown;
	public float radius;
	public void Show()
	{
		isActive = true;
		lastShown = Time.unscaledTime;
		gObject.SetActive(isActive);
	}

	public void Hide()
	{
		isActive = false;
		gObject.SetActive(isActive);
	}

	public void UpdateFloatingText()
	{
		if (!isActive)
			return;

		if (Time.unscaledTime - lastShown >= duration)
			Hide();

		gObject.transform.position += motion * Time.unscaledDeltaTime;
	}

}
