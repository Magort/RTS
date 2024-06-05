using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorial : MonoBehaviour
{
	public List<GameObject> initialTips;
	public GameObject actionsTip;
	public GameObject roundEndTip;
	int tipIndex = 0;

	void Start()
	{
		StartTutorial();
		GameEventsManager.ActionsRolled.AddListener(PlayerRolledActions);
		GameEventsManager.RoundEnded.AddListener(ShowRoundEndTip);
		CombatPanel.Instance.speedPrompt.gameObject.SetActive(false);
		CombatPanel.Instance.speedPrompt.button.gameObject.SetActive(false);
	}

	void StartTutorial()
	{
		initialTips[tipIndex].SetActive(true);
	}

	public void ShowNextInitialTip()
	{
		initialTips[tipIndex++].SetActive(false);
		initialTips[tipIndex].SetActive(true);
		if (tipIndex == initialTips.Count - 2)
		{
			CombatPanel.Instance.speedPrompt.gameObject.SetActive(true);
		}

		if (tipIndex == initialTips.Count - 1)
		{
			CombatPanel.Instance.speedPrompt.button.gameObject.SetActive(true);
		}
	}

	void PlayerRolledActions(Affiliation affiliation)
	{
		if (affiliation == Affiliation.Player)
		{
			StartCoroutine(WaitForRolls());
		}
	}

	IEnumerator WaitForRolls()
	{
		yield return new WaitForSecondsRealtime(1.5f);

		ShowActionsTip();
	}

	void ShowActionsTip()
	{
		actionsTip.SetActive(true);
		GameEventsManager.ActionsRolled.RemoveListener(PlayerRolledActions);
	}

	void ShowRoundEndTip()
	{
		roundEndTip.SetActive(true);
		GameEventsManager.RoundEnded.RemoveListener(ShowRoundEndTip);
	}
}
