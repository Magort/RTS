using System.Collections.Generic;
using UnityEngine;

public class CampaignLevelsContainer : MonoBehaviour
{
	public List<CampaignLevelButton> levelButtons;

	public void PopulateLevelsList(List<LevelData> levels)
	{
		ClearButtons();

		for (int i = 0; i < levels.Count; i++)
		{
			levelButtons[i].PopulateButton(levels[i]);
		}

		gameObject.SetActive(true);
	}

	void ClearButtons()
	{
		foreach (var level in levelButtons)
		{
			level.DepopulateButton();
		}
	}
}
