using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NarrativePanel : MonoBehaviour
{
	public static NarrativePanel Instance;
	public GameObject blockingPanel;

	public Image avatar;
	public TextMeshProUGUI narratorName;
	public TextMeshProUGUI textField;
	public TextMeshProUGUI buttonText;

	public List<NarrativeTextPackage> currentNarration;
	public List<Narrator> narrators;

	int currentText = 0;

	private void Awake()
	{
		Instance = this;
		blockingPanel.SetActive(false);
		gameObject.SetActive(false);
	}

	public void StartNewNarration(List<NarrativeTextPackage> packages)
	{
		GameManager.SwitchPauseState(true);
		gameObject.SetActive(true);
		blockingPanel.SetActive(true);

		currentNarration = packages;
		PopulateText(0);
	}

	public void PopulateText(int index)
	{
		var narrator = narrators.First(n => n.code == currentNarration[index].narrator);

		narratorName.text = narrator.name;
		avatar.sprite = narrator.avatar;
		textField.text = currentNarration[index].content;

		HandleButtonText();
	}

	void HandleButtonText()
	{
		if (currentText >= currentNarration.Count - 1)
			buttonText.text = "End";
		else
			buttonText.text = "Next";
	}

	void EndNarration()
	{
		currentText = 0;
		GameManager.SwitchPauseState(false);
		blockingPanel.SetActive(false);
		gameObject.SetActive(false);
	}

	public void ShowNextText()
	{
		if (currentText >= currentNarration.Count - 1)
		{
			EndNarration();
			return;
		}

		currentText++;
		PopulateText(currentText);
	}
}
