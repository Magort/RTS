using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public List<string> texts;
    public Canvas tutorialCanvas;

    int currentTextIndex = 0;

    void Start()
    {
        GameManager.SwitchPauseState(true);
        textField.text = texts[currentTextIndex];
    }

    public void Next()
    {
        AudioManager.Instance.Play(Sound.Name.Click);
		currentTextIndex++;

		if (currentTextIndex >= texts.Count)
            EndTutorial();

        else
        {
			textField.text = texts[currentTextIndex];
		}
    }

    void EndTutorial()
    {
        GameManager.SwitchPauseState(false);
        tutorialCanvas.gameObject.SetActive(false);
    }

}
