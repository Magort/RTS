using System.Collections.Generic;
using UnityEngine;

public static class TileGrid
{
	public static List<Tile> Tiles = new List<Tile>();

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

	public static List<Tile> GetNeighbouringTiles((int, int) coordinates)
	{
		List<Tile> neighbouringTiles = new();
		var colliders = Physics.OverlapSphere
			(Tiles.Find(tile => tile.coordinates == coordinates).transform.position,
			1.5f,
			LayerMask.GetMask("Tile"));
		foreach (var collider in colliders)
		{
			neighbouringTiles.Add(collider.GetComponent<Tile>());
		}

		return neighbouringTiles;
	}

}
