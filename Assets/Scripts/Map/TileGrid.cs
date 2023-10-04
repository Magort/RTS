using System.Collections.Generic;
using UnityEngine;

public static class TileGrid
{
	public static List<Tile> Tiles = new();
	public static List<Vector2> NeighbouringCoordinates = new()
	{
		new Vector2(-1, 0),
		new Vector2(0, -1),
		new Vector2(1, -1),
		new Vector2(1, 0),
		new Vector2(0, 1),
		new Vector2(-1, 1)
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

		foreach(Vector2 coords in NeighbouringCoordinates)
		{
			var potentialTile = Tiles.Find(nTile => nTile.coordinates == tile.coordinates + coords);

			if (potentialTile != null)
				neighbouringTiles.Add(potentialTile);
		}

		return neighbouringTiles;
	}

}
