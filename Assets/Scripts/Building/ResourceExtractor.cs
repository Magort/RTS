using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceExtractor : Building
{
    public Resource resourceToExtract;
	public int extractAmount;
    private Dictionary<Resource, TileArea.Type> resourceToArea = new()
    {
        {Resource.Food, TileArea.Type.FoodSource },
		{Resource.Wood, TileArea.Type.WoodSource },
		{Resource.Gold, TileArea.Type.GoldSource },
		{Resource.Essence, TileArea.Type.EssenceSource }
	};

	private WaitForSeconds extractionPeriod = new(5);

	private void Start()
	{
		StartCoroutine(TryExtract());
		GameState.AddResourceGrowth(resourceToExtract, extractAmount);
	}

	IEnumerator TryExtract()
	{
		while(true)
		{
			yield return extractionPeriod;

			var target = builtOn.areas.Find(area => area.type == resourceToArea[resourceToExtract]);

			if(target == null)
			{
				GameState.AddResourceGrowth(resourceToExtract, -extractAmount);
				enabled = false;
				break;
			}

			if(target.resourceAmount > extractAmount)
			{
				target.resourceAmount -= extractAmount;

				if(target.resourceAmount < extractAmount)
					target.DepleteResources();
			}
		}
	}
}
