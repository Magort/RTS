using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProducer : Building
{
	public List<Resource> resourcesToProduce;
	public List<float> productionSpeeds;
	private void Start()
	{
		for (int i = 0; i < productionSpeeds.Count; i++)
		{
			StartCoroutine(Produce(resourcesToProduce[i], new(productionSpeeds[i])));
			GameState.AddResourceGrowth(resourcesToProduce[i], 1 / productionSpeeds[i]);
		}
	}

	IEnumerator Produce(Resource resource, WaitForSeconds waiter)
	{
		while (true)
		{
			yield return waiter;

			GameState.AddResource(resource, 1);
		}
	}

	public override string Description()
	{
		string description = "Produces: ";
		for (int i = 0; i < productionSpeeds.Count; i++)
		{
			description += "1<sprite="
			   + IconIDs.resourceToIconID[resourcesToProduce[i]] + ">/" + "<b>" + productionSpeeds[i] + "</b>s ";
		}

		return description;
	}

	public override void SpecialOnBuild(Tile tile, TileArea area)
	{

	}
}
