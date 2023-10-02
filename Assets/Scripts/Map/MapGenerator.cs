using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public Tile mainTile;
	[Header("Prefabs")]
    public GameObject tilePrefab;
	public Tile startingFoodTile;
	public Tile startingWoodTile;
	public Tile startingGoldTile;

	public int debugSize;

	float xOffsetFull = 1.73f;
	float zOffsetFull = 1.5f;

    Vector3 startingPoint = Vector3.zero;

	private void Start()
    {
        GenerateMap(debugSize);
    }

    void GenerateMap(int size)
    {
        var midRowSize = size * 2 + 1;

        for(int i = 0; i < midRowSize; i++)
        {
			if (i != size)
			{
				var tile = Instantiate(tilePrefab, startingPoint - new Vector3(xOffsetFull, 0, 0) * (size - i), Quaternion.identity, transform).GetComponent<Tile>();
				tile.InitializeTile((size, i));
				TileGrid.AddTile(tile);
			}
			else
			{
				mainTile.coordinates = (size, i);
				mainTile.debugCoordinates = new Vector2(size, i);
				TileGrid.AddTile(mainTile);
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
				tile.InitializeTile((size + i, j));
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
				tile.InitializeTile((size - i, j));
				TileGrid.AddTile(tile);
			}
		}

		EnsureStartingResources();
		EnableNeighbouringTiles();
	}

	void EnsureStartingResources()
	{
		var mainTileCoordinates = mainTile.coordinates;

		List<(int, int)> validCoordinates = new()
		{
			(mainTileCoordinates.Item1 -1 , mainTileCoordinates.Item2 - 1),
			(mainTileCoordinates.Item1 -1, mainTileCoordinates.Item2),
			(mainTileCoordinates.Item1, mainTileCoordinates.Item2 - 1),
			(mainTileCoordinates.Item1, mainTileCoordinates.Item2 + 1),
			(mainTileCoordinates.Item1, mainTileCoordinates.Item2 + 1),
			(mainTileCoordinates.Item1 + 1, mainTileCoordinates.Item2)
		};

		var foodCoordinates = validCoordinates[Random.Range(0, validCoordinates.Count)];
		validCoordinates.Remove(foodCoordinates);

		TileGrid.Tiles.Find(tile => tile.coordinates == foodCoordinates).TransformIntoTile(startingFoodTile);

		var woodCoordinates = validCoordinates[Random.Range(0, validCoordinates.Count)];
		validCoordinates.Remove(woodCoordinates);

		TileGrid.Tiles.Find(tile => tile.coordinates == woodCoordinates).TransformIntoTile(startingWoodTile);

		var goldCoordinates = validCoordinates[Random.Range(0, validCoordinates.Count)];
		validCoordinates.Remove(goldCoordinates);

		TileGrid.Tiles.Find(tile => tile.coordinates == goldCoordinates).TransformIntoTile(startingGoldTile);
	}

	void EnableNeighbouringTiles()
	{
		foreach(var tile in TileGrid.GetNeighbouringTiles(mainTile.coordinates))
		{
			tile.BecomeNeighbour();
		}
	}
}
