using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public Tile mainTile;
	public int debugSize;
	[Header("Prefabs")]
    public GameObject tilePrefab;
	public Tile startingFoodTile;
	public Tile startingWoodTile;
	public Tile startingGoldTile;
	public List<MapUnit> neutralMapUnits = new();
	[Header("Hostilities")]
	public int neutralsDensity;


	float xOffsetFull = 1.73f;
	float zOffsetFull = 1.5f;

    Vector3 startingPoint = Vector3.zero;

	private void Start()
    {
        GenerateMap(debugSize);
		TileGrid.Size = debugSize;
		GenerateNeutralUnits();
    }

    void GenerateMap(int size)
    {
        var midRowSize = size * 2 + 1;

        for(int i = 0; i < midRowSize; i++)
        {
			if (i != size)
			{
				var tile = Instantiate(tilePrefab, startingPoint - new Vector3(xOffsetFull, 0, 0) * (size - i), Quaternion.identity, transform).GetComponent<Tile>();
				tile.InitializeTile(new Vector3Int(i, size, (i + size) * -1));
				TileGrid.AddTile(tile);
			}
			else
			{
				TileGrid.AddTile(mainTile);
				mainTile.coordinates = new Vector3Int(size, size, size * -2);
			}
		}

        for(int i = 1; i <= size; i++)
        {
            Vector3 newStartingPoint =
                (startingPoint - new Vector3(xOffsetFull, 0, 0) * size) + new Vector3(xOffsetFull/2, 0, zOffsetFull) * i;

            for(int j = 0; j <= (size * 2) - i; j++)
            {
				var tile = Instantiate(tilePrefab, newStartingPoint + new Vector3(xOffsetFull, 0, 0) * j, Quaternion.identity, transform)
					.GetComponent<Tile>();
				tile.InitializeTile(new Vector3Int(j, size + i, (j + size + i) * -1));
				TileGrid.AddTile(tile);
			}
		}

		for (int i = 1; i <= size; i++)
		{
			Vector3 newStartingPoint =
				(startingPoint - new Vector3(xOffsetFull, 0, 0) * size) + new Vector3(xOffsetFull / 2, 0, -zOffsetFull) * i;

			for (int j = 0; j <= (size * 2) - i; j++)
			{
				var tile = Instantiate(tilePrefab, newStartingPoint + new Vector3(xOffsetFull, 0, 0) * j, Quaternion.identity, transform)
					.GetComponent<Tile>();
				tile.InitializeTile(new Vector3Int(i + j, size - i, (i + j + size - i) * -1));
				TileGrid.AddTile(tile);
			}
		}

		EnsureStartingResources();
		EnableNeighbouringTiles();
	}

	void EnsureStartingResources()
	{
		var potentialTiles = TileGrid.GetNeighbouringTiles(mainTile);

		var foodTile = potentialTiles[Random.Range(0, potentialTiles.Count)];
		potentialTiles.Remove(foodTile);

		TileGrid.Tiles.Find(tile => tile == foodTile).TransformIntoTile(startingFoodTile);

		var woodTile = potentialTiles[Random.Range(0, potentialTiles.Count)];
		potentialTiles.Remove(woodTile);

		TileGrid.Tiles.Find(tile => tile == woodTile).TransformIntoTile(startingWoodTile);

		var goldTile = potentialTiles[Random.Range(0, potentialTiles.Count)];
		potentialTiles.Remove(goldTile);

		TileGrid.Tiles.Find(tile => tile == goldTile).TransformIntoTile(startingGoldTile);
	}

	void GenerateNeutralUnits()
	{
		List<Tile> availableTiles = TileGrid.Tiles.Except(TileGrid.GetNeighbouringTiles(mainTile)).ToList();
		availableTiles.Remove(mainTile);

		var neutralsToSpawn = neutralsDensity * TileGrid.Size;

		for(int i = 0; i < neutralsToSpawn; i++)
		{
			Tile selectedTile = availableTiles[Random.Range(0, availableTiles.Count)];

			selectedTile.MapGenerationAddUnit(GetNeutralUnit(
				Mathf.Max(Mathf.Abs(selectedTile.coordinates.x - TileGrid.Size),
				Mathf.Abs(selectedTile.coordinates.y - TileGrid.Size))));

			availableTiles.Remove(selectedTile);
		}
	}	

	MapUnit GetNeutralUnit(int highestCoordinate)
	{
		int index = Mathf.FloorToInt(highestCoordinate / 3);
		return neutralMapUnits[index];
	}

	void EnableNeighbouringTiles()
	{
		foreach(var tile in TileGrid.GetNeighbouringTiles(mainTile))
		{
			tile.BecomeNeighbour();
		}
	}
}
