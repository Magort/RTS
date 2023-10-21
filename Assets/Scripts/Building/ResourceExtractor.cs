using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceExtractor : Building
{
    public Resource resourceToExtract;
	public float extractSpeed;
    private Dictionary<Resource, TileArea.Type> resourceToArea = new()
    {
        {Resource.Food, TileArea.Type.FoodSource },
		{Resource.Wood, TileArea.Type.WoodSource },
		{Resource.Gold, TileArea.Type.GoldSource },
		{Resource.Essence, TileArea.Type.EssenceSource }
	};

	private WaitForSeconds extractionPeriod;

	public override void SpecialOnBuild(Tile tile, TileArea area)
	{
	}

	private void Start()
	{
		extractionPeriod = new(extractSpeed);
		StartCoroutine(TryExtract());
		GameState.AddResourceGrowth(resourceToExtract, 1 / extractSpeed);
	}

	IEnumerator TryExtract()
	{
		while(true)
		{
			yield return extractionPeriod;

			var target = builtOn.areas.Find(area => area.type == resourceToArea[resourceToExtract]);

			if(target != null)
			{
				target.resourceAmount--;
				GameState.AddResource(resourceToExtract, 1);
			}
			else
			{
				GameState.AddResourceGrowth(resourceToExtract, 1 / extractSpeed);
				yield break;
			}
		}
	}
}
