using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveReward
{
	public List<Building.Requirements.ResourceRequirement> resources;
	public List<Building.Code> buildings;
	public List<Vector3Int> tiles;

	public void GrantRewards()
	{
		foreach (var resource in resources)
		{
			GameState.AddResource(resource.resource, resource.amount);
		}

		foreach (var building in buildings)
		{
			BuildingHandler.Instance.availableBuildings.Add(building);
		}

		foreach (var tile in tiles)
		{
			TileGrid.Tiles.Find(t => t.data.navigationCoordinates == tile).Reveal();
		}
	}
}
