using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionObjective", menuName = "MissionObjectives/TileDiscover", order = 3)]
public class MOTileDiscover : MissionObjective
{
	public List<Vector3Int> tilesByCoords = new();
	List<Tile> tiles = new();

	public override bool ConditionsMet()
	{
		foreach (var tile in tiles)
		{
			if(!tile.data.discovered)
				return false;
		}

		return true;
	}

	public override void Innit()
	{
		tilesByCoords.ForEach(tile => tiles
		.Add(TileGrid.Tiles.Find(searchedTiled => searchedTiled.data.navigationCoordinates == tile)));

		GameEventsManager.TileDiscovered.AddListener(CheckDiscoveredTile);
		Debug.Log("innit");
	}

	void CheckDiscoveredTile(Vector3Int tile)
	{
		if (tilesByCoords.Contains(tile))
		{
			ProgressQuantity();
		}
	}

	public override void Clear()
	{
		GameEventsManager.TileDiscovered.RemoveListener(CheckDiscoveredTile);
	}
}
