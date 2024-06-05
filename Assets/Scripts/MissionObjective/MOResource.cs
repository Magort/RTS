using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionObjective", menuName = "MissionObjectives/Resource", order = 4)]
public class MOResource : MissionObjective
{
	public List<Building.Requirements.ResourceRequirement> resourceRequirements;

	readonly WaitForSeconds waiter = new(1);

	public override void Clear()
	{

	}

	public override bool ConditionsMet()
	{
		foreach (var requirement in resourceRequirements)
		{
			if (GameState.Resources[requirement.resource] < requirement.amount)
				return false;
		}

		return true;
	}

	public override void Innit()
	{
		ObjectivesManager.Instance.StartCoroutine(CheckResources());
	}

	IEnumerator CheckResources()
	{
		while (true)
		{
			yield return waiter;

			if (ConditionsMet())
			{
				ProgressQuantity();
				break;
			}
		}
	}
}
