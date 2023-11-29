using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public static class TileGrid
{
	public static int Size;
	public static Tile MainTile;
	public static List<Tile> WinTargetTiles = new();
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
	public static bool IsNextToPlyerKingdom(Tile tile)
	{
		return GetNeighbouringTiles(tile)
			.Where(tile => tile.affiliation == Affiliation.Player).ToList().Count == 0;
	}

	public static Tile GetClosestTileOfType(Tile startingTile, Affiliation targetAffiliation)
	{
		return GetClosest(startingTile, Tiles.Where(tile => tile.affiliation == targetAffiliation).ToList());
	}

	static Tile GetClosest(Tile startingTile, List<Tile> availableTiles)
	{
		int minDistance = 100;
		Tile closestTile = null;

		foreach(Tile tile in availableTiles)
		{
			int estimatedDistance =
				Mathf.Max(Mathf.Abs(startingTile.coordinates.z - tile.coordinates.z),
				Mathf.Abs(startingTile.coordinates.x - tile.coordinates.x),
				Mathf.Abs(startingTile.coordinates.y - tile.coordinates.y));

			if(estimatedDistance < minDistance)
			{
				closestTile = tile;
				minDistance = estimatedDistance;
			}
		}

		return closestTile;
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
