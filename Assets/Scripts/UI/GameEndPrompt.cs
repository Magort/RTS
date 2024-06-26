using TMPro;
using UnityEngine;

public class GameEndPrompt : MonoBehaviour
{
	public TextMeshProUGUI title;
	public TextMeshProUGUI body;

	public string winTitle;
	public string loseTitle;
	[TextArea] public string winBody;
	[TextArea] public string loseBody;

	public void PopulatePrompt(bool win)
	{
		gameObject.SetActive(true);
		GameManager.SwitchPauseState(true);
		if (win)
		{
			title.text = winTitle;
			body.text = winBody;
		}
		else
		{
			title.text = loseTitle;
			body.text = loseBody;
		}
	}
}
