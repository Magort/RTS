using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public TextMeshProUGUI actionText;
	public Button button;

    public void PopulateButton(CombatAction action, Affiliation affiliation)
    {
		actionText.text = "";
		foreach (var subAction in action.subActions)
		{
			actionText.text += subAction.value
							+ "<sprite=" + IconIDs.effectToIconID[subAction.effect] + ">\n";
		}

		if (affiliation != Affiliation.Player)
			SetInteractable(false);

		else
			SetInteractable(true);

		gameObject.SetActive(true);
	}

	public void SetInteractable(bool status)
	{
		button.interactable = status;
	}

	public void HideButton()
	{
		gameObject.SetActive(false);
	}
}
