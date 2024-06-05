using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceExtractor : Building
{
	public Resource resourceToExtract;
	public float extractInterval;
	private Dictionary<Resource, TileArea.Type> resourceToArea = new()
	{
		{Resource.Food, TileArea.Type.FoodSource },
		{Resource.Wood, TileArea.Type.WoodSource },
		{Resource.Gold, TileArea.Type.GoldSource },
		{Resource.Essence, TileArea.Type.EssenceSource }
	};

	TileArea currentTarget = null;

	private WaitForSeconds extractionPeriod;

	public override string Description()
	{
		return "Extracts 1<sprite="
			+ IconIDs.resourceToIconID[resourceToExtract]
			+ "> from the tile every <b>" + extractInterval + "</b> seconds.";
	}

	public override void SpecialOnBuild(Tile tile, TileArea area)
	{
	}

	private void Start()
	{
		extractionPeriod = new(extractInterval);
		StartCoroutine(TryExtract());
		GameState.AddResourceGrowth(resourceToExtract, 1 / extractInterval);
	}

	IEnumerator TryExtract()
	{
		while (true)
		{
			yield return extractionPeriod;

			if (currentTarget == null)
				currentTarget = builtOn.areas.Find(area => area.data.type == resourceToArea[resourceToExtract]);

			if (currentTarget != null)
			{
				currentTarget.data.resourceAmount--;
				GameState.AddResource(resourceToExtract, 1);
				if (currentTarget.data.resourceAmount <= 0)
				{
					currentTarget.DepleteResources();
					currentTarget = null;
				}
			}
			else
			{
				GameState.AddResourceGrowth(resourceToExtract, -1 / extractInterval);
				builtOn.areas.Find(area => area.data.building == code && area.data.type == TileArea.Type.Building).RemoveBuilding();
				yield break;
			}
		}
	}
}
