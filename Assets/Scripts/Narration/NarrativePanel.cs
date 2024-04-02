using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NarrativePanel : MonoBehaviour
{
    public static NarrativePanel Instance;

    public TextMeshProUGUI narratorName;
    public TextMeshProUGUI textField;
    public TextMeshProUGUI buttonText;

    public List<NarrativeTextPackage> currentNarration;

    int currentText = 0;

	private void Awake()
	{
		Instance = this;
        gameObject.SetActive(false);
	}

	public void StartNewNarration(List<NarrativeTextPackage> packages)
    {
        GameManager.SwitchPauseState(true);
        gameObject.SetActive(true);

		currentNarration = packages;
        PopulateText(0);
    }

    public void PopulateText(int index)
    {
        narratorName.text = currentNarration[index].narrator;
        textField.text = currentNarration[index].content;

        HandleButtonText();
	}

    void HandleButtonText()
    {
		if (currentText >= currentNarration.Count)
			buttonText.text = "End";
		else
			buttonText.text = "Next";
	}

    void EndNarration()
    {
		GameManager.SwitchPauseState(false);
		gameObject.SetActive(false);
	}

    public void ShowNextText()
    {
        if (currentText >= currentNarration.Count)
        {
            EndNarration();
		}

		currentText++;
		PopulateText(currentText);
    }
}
