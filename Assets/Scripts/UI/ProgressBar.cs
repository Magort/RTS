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

		transform.position = Camera.main.WorldToScreenPoint(target.position);
		
		timer += Time.deltaTime;
		progressBar.fillAmount = timer / duration;

		if (timer >= duration)
			ClearBar();
	}

	public void ShowProgress(Transform target, float time, string text)
	{
		duration = time;
		this.target = target;
		textBar.text = text;
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
