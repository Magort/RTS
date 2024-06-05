using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionObjective", menuName = "MissionObjectives/TileControl", order = 1)]
public class MOTileControl : MissionObjective
{
	public List<Vector3Int> requiredTilesByCoords = new();
	List<Tile> requiredTiles = new();

	public override bool ConditionsMet()
	{
		foreach (var tile in requiredTiles)
		{
			if (tile.data.units.Count > 0)
			{
				if (tile.data.units[0].affiliation != Affiliation.Player)
					return false;
			}
		}

		return true;
	}

	public override void Innit()
	{
		requiredTilesByCoords.ForEach(tile => requiredTiles
		.Add(TileGrid.Tiles.Find(searchedTiled => searchedTiled.data.navigationCoordinates == tile)));

		GameEventsManager.TileControlled.AddListener(CheckControlledTile);
	}

	void CheckControlledTile(Vector3Int tile)
	{
		if (requiredTilesByCoords.Contains(tile))
			ProgressQuantity();
	}

	public override void Clear()
	{
		GameEventsManager.TileControlled.RemoveListener(CheckControlledTile);
	}
}
