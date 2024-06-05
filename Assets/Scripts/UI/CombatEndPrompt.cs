using TMPro;
using UnityEngine;

public class CombatEndPrompt : MonoBehaviour
{
	public TextMeshProUGUI title;
	public TextMeshProUGUI body;

	[Header("Texts")]
	public string playerWinTitle;
	public string opponentWinTitle;
	[TextArea] public string playerWinBody;
	[TextArea] public string opponentWinBody;

	public void ShowPrompt(Affiliation looser)
	{
		switch (looser)
		{
			case Affiliation.Player:
				title.text = opponentWinTitle;
				body.text = opponentWinBody;
				break;

			default:
				title.text = playerWinTitle;
				body.text = playerWinBody;
				break;
		}

		gameObject.SetActive(true);
	}
}
