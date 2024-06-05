using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public static MapGenerator Instance;

	[Header("Prefabs")]
	public GameObject neutralTilePrefab;
	public GameObject fireTilePrefab;
	public GameObject waterTilePrefab;
	public GameObject airTilePrefab;
	public GameObject earthTilePrefab;

	[Header("Random Map")]
	public Tile startingFoodTile;
	public Tile startingWoodTile;
	public Tile startingGoldTile;
	public Tile startingEssenceTile;
	public List<NeutralMapUnitSpawnData> neutralMapUnits = new();
	public List<Tile> enemyTiles = new();

	Dictionary<Biome, GameObject> biomeToTilePrefab = new();

	[System.Serializable]
	public class NeutralMapUnitSpawnData
	{
		public List<MapUnit> units;
		public int tier;
	}

	public static float xOffsetFull = 1.73f;
	public static float zOffsetFull = 1.5f;

	Vector3 startingPoint = Vector3.zero;

	private void Awake()
	{
		Instance = this;
		MapPrefabs();
	}

	void MapPrefabs()
	{
		biomeToTilePrefab = new()
		{
			{ Biome.Neutral, neutralTilePrefab },
			{ Biome.Fire, fireTilePrefab },
			{ Biome.Water, waterTilePrefab },
			{ Biome.Air, airTilePrefab },
			{ Biome.Earth, earthTilePrefab }
		};
	}

	public void LoadMap(List<TileData> tilesData)
	{
		foreach (var tileData in tilesData)
		{
			var tile = Instantiate(biomeToTilePrefab[tileData.biome], tileData.spaceCoordinates, Quaternion.identity).GetComponent<Tile>();

			tile.LoadFromData(tileData);
			tile.transform.parent = gameObject.transform;
			TileGrid.AddTile(tile);
		}
	}

	public void GenerateRandomMap(int size, Biome biome)
	{
		var tilePrefab = biomeToTilePrefab[biome];
		var midRowSize = size * 2 + 1;
		TileGrid.Size = size;

		for (int i = 0; i < midRowSize; i++)
		{
			if (i != size)
			{
				var tile = Instantiate(tilePrefab, startingPoint - new Vector3(xOffsetFull, 0, 0) * (size - i), Quaternion.identity, transform).GetComponent<Tile>();
				tile.InitializeTile(new Vector3Int(i, size, (i + size) * -1));
				TileGrid.AddTile(tile);
			}
			else
			{
				SetMainTile(Instantiate(tilePrefab, startingPoint - new Vector3(xOffsetFull, 0, 0) * (size - i), Quaternion.identity, transform).GetComponent<Tile>(), size);
			}
		}

		for (int i = 1; i <= size; i++)
		{
			Vector3 newStartingPoint =
				(startingPoint - new Vector3(xOffsetFull, 0, 0) * size) + new Vector3(xOffsetFull / 2, 0, zOffsetFull) * i;

			for (int j = 0; j <= (size * 2) - i; j++)
			{
				var tile = Instantiate(tilePrefab, newStartingPoint + new Vector3(xOffsetFull, 0, 0) * j, Quaternion.identity, transform)
					.GetComponent<Tile>();
				tile.InitializeTile(new(j, size + i, (j + size + i) * -1));
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
				tile.InitializeTile(new(i + j, size - i, (i + j + size - i) * -1));
				TileGrid.AddTile(tile);
			}
		}

		EnsureStartingResources();
		EnableNeighbouringTiles();
		GenerateNeutralUnits();
		GenerateEnemyBases();
	}

	void SetMainTile(Tile tile, int size)
	{
		TileGrid.MainTile = tile;
		tile.ChangeAffiliation(Affiliation.Player);
		tile.InitializeTile(new(size, size, -size * 2));
	}

	void EnsureStartingResources()
	{
		var potentialTiles = TileGrid.GetNeighbouringTiles(TileGrid.MainTile);

		var foodTile = potentialTiles[Random.Range(0, potentialTiles.Count)];
		potentialTiles.Remove(foodTile);

		TileGrid.Tiles.Find(tile => tile == foodTile).TransformIntoTile(startingFoodTile);

		var woodTile = potentialTiles[Random.Range(0, potentialTiles.Count)];
		potentialTiles.Remove(woodTile);

		TileGrid.Tiles.Find(tile => tile == woodTile).TransformIntoTile(startingWoodTile);

		var goldTile = potentialTiles[Random.Range(0, potentialTiles.Count)];
		potentialTiles.Remove(goldTile);

		TileGrid.Tiles.Find(tile => tile == goldTile).TransformIntoTile(startingGoldTile);

		var roll = Random.Range(0, TileGrid.NeighbouringCoordinates.Count);

		TileGrid.Tiles.Find(tile =>
			tile.data.navigationCoordinates ==
			TileGrid.MainTile.data.navigationCoordinates + TileGrid.NeighbouringCoordinates[roll] * 2)
			.TransformIntoTile(startingEssenceTile);
	}

	void GenerateNeutralUnits()
	{
		List<Tile> availableTiles = TileGrid.Tiles.ToList();
		availableTiles.Remove(TileGrid.MainTile);
		availableTiles = availableTiles.Except(TileGrid.GetNeighbouringTiles(TileGrid.MainTile)).ToList();

		var neutralsToSpawn = 0;

		for (int i = 1; i <= TileGrid.Size; i++)
		{
			neutralsToSpawn += 3 * i;
		}

		for (int i = 0; i < neutralsToSpawn; i++)
		{
			Tile selectedTile = availableTiles[Random.Range(0, availableTiles.Count)];

			var unit = GetNeutralUnit(
				Mathf.Max(Mathf.Abs(selectedTile.data.navigationCoordinates.x - TileGrid.Size),
				Mathf.Abs(selectedTile.data.navigationCoordinates.y - TileGrid.Size)));

			selectedTile.MapGenerationAddUnit(unit);
			EnemyBehaviourHandler.Instance.AddUnit(unit);

			availableTiles.Remove(selectedTile);
		}
	}

	MapUnit GetNeutralUnit(int highestCoordinate)
	{
		int tier = Mathf.FloorToInt(highestCoordinate / 4);
		var unitsOnTier = neutralMapUnits.Find(units => units.tier == tier);
		return unitsOnTier.units[Random.Range(0, unitsOnTier.units.Count)];
	}

	void GenerateEnemyBases()
	{
		List<Vector3> validCoordinates = new()
		{
			new(9, 2, -11),
			new(1, 10, -11),
			new(11, 12, -23),
			new(4, 16, -20),
			new(16, 4, -20),
			new(16, 2, -18)
		};

		List<Tile> validTiles = TileGrid.Tiles.Where(tile => validCoordinates.Contains(tile.data.navigationCoordinates)).ToList();

		for (int i = 0; i < enemyTiles.Count; i++)
		{
			var roll = Random.Range(0, validTiles.Count);

			var worldCoordinates = validTiles[roll].transform.position;
			var coodrdinates = validTiles[roll].data.navigationCoordinates;
			TileGrid.Tiles.Remove(validTiles[i]);
			Destroy(validTiles[i].gameObject);

			var newTile = Instantiate(enemyTiles[i], worldCoordinates, Quaternion.identity);
			newTile.data.navigationCoordinates = coodrdinates;
			TileGrid.AddTile(newTile);
			newTile.ChangeAffiliation(Affiliation.Enemy);

			TileGrid.GetNeighbouringTiles(newTile)
				.ForEach(tile => tile.ChangeAffiliation(Affiliation.Enemy));

			validTiles.RemoveAt(roll);
		}
	}

	void EnableNeighbouringTiles()
	{
		foreach (var tile in TileGrid.GetNeighbouringTiles(TileGrid.MainTile))
		{
			tile.BecomeNeighbour();
		}
	}
}
