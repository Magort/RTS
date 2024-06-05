using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : Building
{
	public Resource resourceToSpawn;
	public float spawnInterval;
	private WaitForSeconds spawnPeriod;
	private Dictionary<Resource, TileArea.Type> resourceToArea = new()
	{
		{ Resource.Food, TileArea.Type.FoodSource },
		{ Resource.Wood, TileArea.Type.WoodSource },
		{ Resource.Gold, TileArea.Type.GoldSource },
		{ Resource.Essence, TileArea.Type.EssenceSource }
	};

	TileArea target = null;

	public override string Description()
	{
		return "Spawns 1<sprite="
			+ IconIDs.resourceToIconID[resourceToSpawn]
			+ "> on the tile every <b>" + spawnInterval + "</b> seconds.";
	}

	public override void SpecialOnBuild(Tile tile, TileArea area)
	{
	}

	private void Start()
	{
		spawnPeriod = new(spawnInterval);
		target = builtOn.areas.Find(area => area.data.type == resourceToArea[resourceToSpawn]);
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn()
	{
		while (true)
		{
			yield return spawnPeriod;

			target.data.resourceAmount++;
		}
	}
}
