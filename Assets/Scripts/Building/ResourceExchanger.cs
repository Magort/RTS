using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceExchanger : Building
{
	public List<Resource> resourcesToGain;
	public List<Resource> resourcesToLose;
	public List<float> gainAmounts;
	public float productionSpeed;

	void Start()
	{
		StartCoroutine(Exchange());
		foreach (var resource in resourcesToGain)
		{
			GameState.AddResourceGrowth(resource, 1 / productionSpeed);
		}
		foreach (var resource in resourcesToLose)
		{
			GameState.AddResourceGrowth(resource, -1 / productionSpeed);
		}
	}

	IEnumerator Exchange()
	{
		WaitForSeconds waiter = new(productionSpeed);

		while (true)
		{
			yield return waiter;

			if (TryPayResources())
			{
				for (int i = 0; i < resourcesToGain.Count; i++)
				{
					GameState.AddResource(resourcesToGain[i], gainAmounts[i]);
				}
			}
		}
	}

	bool TryPayResources()
	{
		foreach (Resource resource in resourcesToLose)
		{
			if (GameState.Resources[resource] <= 0)
			{
				return false;
			}
		}

		foreach (Resource resource in resourcesToLose)
		{
			if (GameState.Resources[resource] <= 0)
			{
				GameState.AddResource(resource, -1);
			}
		}

		return true;
	}

	public override string Description()
	{
		string description = "Trades: ";
		for (int i = 0; i < resourcesToLose.Count; i++)
		{
			description += "1<sprite="
			   + IconIDs.resourceToIconID[resourcesToLose[i]] + ">/";
		}
		description += "\nFor: ";
		for (int i = 0; i < resourcesToGain.Count; i++)
		{
			description += "1<sprite="
			   + IconIDs.resourceToIconID[resourcesToGain[i]] + ">/";
		}
		description += "every " + productionSpeed + "s";

		return description;
	}

	public override void SpecialOnBuild(Tile tile, TileArea area)
	{

	}
}
