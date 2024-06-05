using UnityEngine;

public class MapEditorLevelGenerator : MonoBehaviour
{
	public GameObject tilePrefab;
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
		TileGrid.Tiles = new();

		var midRowSize = size * 2 + 1;

		for (int i = 0; i < midRowSize; i++)
		{
			var tile = Instantiate(tilePrefab, startingPoint - new Vector3(xOffsetFull, 0, 0) * (size - i), Quaternion.identity, transform).GetComponent<Tile>();
			tile.data.navigationCoordinates = new Vector3Int(i, size, (i + size) * -1);
			TileGrid.AddTile(tile);
			ShowTile(tile);
		}

		for (int i = 1; i <= size; i++)
		{
			Vector3 newStartingPoint =
				(startingPoint - new Vector3(xOffsetFull, 0, 0) * size) + new Vector3(xOffsetFull / 2, 0, zOffsetFull) * i;

			for (int j = 0; j <= (size * 2) - i; j++)
			{
				var tile = Instantiate(tilePrefab, newStartingPoint + new Vector3(xOffsetFull, 0, 0) * j, Quaternion.identity, transform)
					.GetComponent<Tile>();
				tile.data.navigationCoordinates = new Vector3Int(j, size + i, (j + size + i) * -1);
				TileGrid.AddTile(tile);
				ShowTile(tile);
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
				tile.data.navigationCoordinates = new Vector3Int(i + j, size - i, (i + j + size - i) * -1);
				TileGrid.AddTile(tile);
				ShowTile(tile);
			}
		}
	}

	void ShowTile(Tile tile)
	{
		tile.gameObject.SetActive(true);
		tile.container.SetActive(true);
	}
}
