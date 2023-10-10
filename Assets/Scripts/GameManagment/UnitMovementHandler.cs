using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class UnitMovementHandler : MonoBehaviour
{
    public static UnitMovementHandler Instance;
	public LineRenderer pathLine;
	List<MapUnit> selectedUnits = new();

	int stepTime = 3;

	Dictionary<MapUnit, Coroutine> movementDictionary = new();

	private void Awake()
    {
        Instance = this;
    }

	public void SelectUnit(MapUnit unit)
	{
		selectedUnits.Add(unit);
		ShowPath(unit.path);
	}

	public void Deselect()
	{
		selectedUnits = new();
		HidePath();
	}

	public void TryMove(Tile destinationTile)
	{
		if (selectedUnits.Count == 0)
			return;

		var path = CreatePath(destinationTile);
		
		foreach (var unit in selectedUnits)
		{ 	
			unit.path = path;

			if (movementDictionary.ContainsKey(unit))
			{
				StopCoroutine(movementDictionary[unit]);
				movementDictionary[unit] = StartCoroutine(Move(unit));
			}
			else
				movementDictionary.Add(unit, StartCoroutine(Move(unit)));
		}

		ShowPath(path);
	}

	public void StopMovement(MapUnit unit)
	{
		StopCoroutine(movementDictionary[unit]);
		movementDictionary.Remove(unit);
	}

	IEnumerator Move(MapUnit unit)
	{
		var waiter = new WaitForSeconds(stepTime);

		while(true)
		{
			yield return waiter;

			unit.path[0].RemoveUnit(unit);
			unit.path[1].AddUnit(unit);
			unit.path.RemoveAt(0);
			if(selectedUnits.Contains(unit))
			{
				ShowPath(unit.path);
			}

			if(unit.path.Count == 1)
			{
				unit.path.RemoveAt(0);
				break;
			}
		}

		movementDictionary.Remove(unit);
	}

	public List<Tile> CreatePath(Tile destinationTile)
	{
		if (ContextMenu.Instance.SelectedTile == null)
			return new();

		ClearNavigationParameters();

		return FindPath(selectedUnits[0].currentTile, destinationTile);
    }

	public void ShowPath(List<Tile> path)
	{
		List<Vector3> worldCoordinates = new();

		foreach (Tile tile in path)
		{
			worldCoordinates.Add(tile.transform.position + new Vector3(0, 0.2f, 0));
		}

		pathLine.positionCount = worldCoordinates.Count;
		pathLine.SetPositions(worldCoordinates.ToArray());
	}

	public void HidePath()
	{
		pathLine.positionCount = 0;
	}

	void ClearNavigationParameters()
	{
		foreach (Tile tile in TileGrid.Tiles.Where(tile => tile.neighbour))
		{
			tile.g = 0;
			tile.h = 0;
			tile.F = 0;
		}
	}

	public static List<Tile> FindPath(Tile startPoint, Tile endPoint)
	{
		List<Tile> openPathTiles = new List<Tile>();
		List<Tile> closedPathTiles = new List<Tile>();

		Tile currentTile = startPoint;

		currentTile.g = 0;
		currentTile.h = (int)GetEstimatedPathCost(startPoint.coordinates, endPoint.coordinates);

		openPathTiles.Add(currentTile);

		while (openPathTiles.Count != 0)
		{
			currentTile.F = currentTile.g + currentTile.h;
			openPathTiles = openPathTiles.OrderBy(x => x.F).ThenByDescending(x => x.g).ToList();
			openPathTiles.Reverse();
			currentTile = openPathTiles[0];

			openPathTiles.Remove(currentTile);
			closedPathTiles.Add(currentTile);

			int g = currentTile.g + 1;

			if (closedPathTiles.Contains(endPoint))
			{
				break;
			}

			foreach (Tile adjacentTile in TileGrid.GetNeighbouringTiles(currentTile))
			{
				if (!adjacentTile.neighbour)
				{
					continue;
				}

				if (closedPathTiles.Contains(adjacentTile))
				{
					continue;
				}

				if (!(openPathTiles.Contains(adjacentTile)))
				{
					adjacentTile.g = g;
					adjacentTile.h = GetEstimatedPathCost(adjacentTile.coordinates, endPoint.coordinates);
					openPathTiles.Add(adjacentTile);
				}

				else if (adjacentTile.F > g + adjacentTile.h)
				{
					adjacentTile.g = g;
				}
			}
		}

		List<Tile> finalPathTiles = new List<Tile>();

		if (closedPathTiles.Contains(endPoint))
		{
			currentTile = endPoint;
			finalPathTiles.Add(currentTile);

			for (int i = endPoint.g - 1; i >= 0; i--)
			{
				currentTile = closedPathTiles.Find(x => x.g == i && TileGrid.GetNeighbouringTiles(currentTile).Contains(x));
				finalPathTiles.Add(currentTile);
			}

			finalPathTiles.Reverse();
		}

		return finalPathTiles;
	}
	protected static int GetEstimatedPathCost(Vector3Int startPosition, Vector3Int targetPosition)
	{
		return Mathf.Max(Mathf.Abs(startPosition.z - targetPosition.z), Mathf.Abs(startPosition.x - targetPosition.x), Mathf.Abs(startPosition.y - targetPosition.y));
	}
}
