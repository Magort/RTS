using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	public Transform target;
	public Image progressBar;
	public TextMeshProUGUI textBar;

	float timer = 0;
	float duration = 0;

	private void Update()
	{
		if (!target)
			return;

		UpdatePosition();
		
		timer += Time.deltaTime;

		UpdateFill();

		if (timer >= duration)
			ClearBar();
	}

	void UpdatePosition()
	{
		transform.position = Camera.main.WorldToScreenPoint(target.position);
	}

	void UpdateFill()
	{
		progressBar.fillAmount = timer / duration;
	}

	public void ShowProgress(Transform target, float time, string text)
	{
		duration = time;
		this.target = target;
		textBar.text = text;
		UpdatePosition();
		UpdateFill();
		gameObject.SetActive(true);
	}

	void ClearBar()
	{
		timer = 0;
		target = null;
		textBar.text = "";
		gameObject.SetActive(false);
	}
}
