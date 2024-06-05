using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MissionObjective : ScriptableObject
{
	public int quantityToPass;
	public int timeToPass;
	public string title;
	public string description;

	public List<NarrativeTextPackage> narration = new();
	public ObjectiveReward reward;

	public void ProgressQuantity()
	{
		ObjectivesManager.Instance.ProgressQuantity();
	}
	public abstract bool ConditionsMet();
	public abstract void Innit();
	public abstract void Clear();
}
