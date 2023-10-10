using System.Collections.Generic;
using UnityEngine;

public static class TileGrid
{
	public static List<Tile> Tiles = new();
	public static List<Vector3Int> NeighbouringCoordinates = new()
	{
		new Vector3Int(-1, 0, 1),
		new Vector3Int(0, -1, 1),
		new Vector3Int(1, -1, 0),
		new Vector3Int(1, 0, -1),
		new Vector3Int(0, 1, -1),
		new Vector3Int(-1, 1, 0)
	};

	public static void AddTile(Tile tile)
	{
		Tiles.Add(tile);
	}

	public static int GetAreaTypeCount(List<TileArea.Type> types, TileArea.Type searchedType)
	{
		int count = 0;
		foreach (TileArea.Type type in types)
		{
			if (type == searchedType)
				count++;
		}

		return count;
	}

	public static List<Tile> GetNeighbouringTiles(Tile tile)
	{
		List<Tile> neighbouringTiles = new();

		foreach(Vector3Int coords in NeighbouringCoordinates)
		{
			var potentialTile = Tiles.Find(nTile => nTile.coordinates == tile.coordinates + coords);

			if (potentialTile != null)
				neighbouringTiles.Add(potentialTile);
		}

		return neighbouringTiles;
	}

}
