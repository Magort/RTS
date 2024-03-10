using TMPro;
using UnityEngine;

public class MissionObjectivePanel : MonoBehaviour
{
	public static MissionObjectivePanel instance;

    public TextMeshProUGUI objectiveTitle;
    public TextMeshProUGUI objectiveDescription;

	private void Start()
	{
		instance = this;
	}

	public void UpdateDisplay()
    {
		var currentObjective = ObjectivesManager.GetCurrentObjective();

		objectiveTitle.text = currentObjective.title;
		objectiveDescription.text = GenerateDescription(currentObjective);
    }

	string GenerateDescription(MissionObjective objective)
	{
		string dscr = "";

		dscr += objective.description;

		if (objective.quantityToPass > 0)
		{
			dscr += " (" + ObjectivesManager.currentQuantity + "/" + objective.quantityToPass + ")";
		}

		if(objective.timeToPass > 0)
		{
			dscr += " (" + ObjectivesManager.currentTimer + "/" + objective.timeToPass + ")";
		}

		return dscr;
	}
}
