using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
	public static ObjectivesManager Instance;

	public List<MissionObjective> missionObjectives;

	public int currentQuantity = 0;
	public int currentTimer = 0;
	public int currentObjective = 0;

	readonly WaitForSeconds waiter = new(1);

	private void Awake()
	{
		Instance = this;
	}

	public void LoadObjectives(List<MissionObjective> objectives)
	{
		missionObjectives = objectives;

		StartCoroutine(DelayObjectives());
	}

	IEnumerator DelayObjectives()
	{
		yield return new WaitForSecondsRealtime(2);
		MissionObjectivePanel.instance.gameObject.SetActive(true);
		SetObjective(0);
	}

	public MissionObjective GetCurrentObjective()
	{
		return missionObjectives[currentObjective];
	}

	public void ProgressQuantity()
	{
		currentQuantity++;

		if (currentQuantity >= missionObjectives[currentObjective].quantityToPass)
			CompleteObjective();

		MissionObjectivePanel.instance.UpdateDisplay();
	}

	public void CompleteObjective()
	{
		missionObjectives[currentObjective].reward.GrantRewards();

		if (currentObjective == missionObjectives.Count - 1)
		{
			GameEndHandler.Instance.WinGame();
		}
		else
		{
			ClearObjective();
			SetObjective(currentObjective + 1);
		}
	}

	public void SetObjective(int index)
	{
		currentObjective = index;
		currentQuantity = 0;

		if (missionObjectives[currentObjective].timeToPass > 0)
			StartCoroutine(ProgressOverTime());

		missionObjectives[currentObjective].Innit();
		MissionObjectivePanel.instance.UpdateDisplay();

		if (missionObjectives[currentObjective].narration.Count > 0)
		{
			NarrativePanel.Instance.StartNewNarration(missionObjectives[currentObjective].narration);
		}
	}

	public void ClearObjective()
	{
		missionObjectives[currentObjective].Clear();
	}

	IEnumerator ProgressOverTime()
	{
		currentTimer = 0;

		while (true)
		{
			if (currentTimer >= missionObjectives[currentObjective].timeToPass)
			{
				CompleteObjective();
				break;
			}

			yield return waiter;

			if (missionObjectives[currentObjective].ConditionsMet())
			{
				currentTimer++;
				MissionObjectivePanel.instance.UpdateDisplay();
			}
		}
	}
}
