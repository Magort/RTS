using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProducer : Building
{
    public List<Resource> resourcesToProduce;
    public float productionSpeed;

	private WaitForSeconds productionPeriod;

	private void Start()
	{
		productionPeriod = new(productionSpeed);
		StartCoroutine(Produce());

		foreach(var resource in resourcesToProduce)
			GameState.AddResourceGrowth(resource, 1 / productionSpeed);
	}

	IEnumerator Produce()
	{
		while (true)
		{
			yield return productionPeriod;
			foreach (var resource in resourcesToProduce)	
				GameState.AddResource(resource, 1);
		}
	}

	public override string Description()
    {
		string description = "Produces ";
		foreach (var resource in resourcesToProduce)
		{
			description += "1<sprite="
			   + IconIDs.resourceToIconID[resource] + "> ";
		}

		description += "every <b>" + productionSpeed + "</b> seconds.";

		return description;
	}

    public override void SpecialOnBuild(Tile tile, TileArea area)
    {
        
    }
}
