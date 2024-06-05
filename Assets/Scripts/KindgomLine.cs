using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KindgomLine : MonoBehaviour
{
	public static KindgomLine Instance;

	private void Awake()
	{
		Instance = this;
	}
	public void ChangeKingdomLine(Tile tile, bool add)
	{
		List<Tile> tilesToUpdate = TileGrid.GetNeighbouringTiles(tile)
			.Where(neighbourTile => neighbourTile.data.affiliation == tile.data.affiliation).ToList();

		foreach (TileArea area in tile.areas)
		{
			area.lineRenderer.enabled = add;
		}

		foreach (Tile nTile in tilesToUpdate)
		{
			tile.areas[TileGrid.NeighbouringCoordinates.IndexOf(nTile.data.navigationCoordinates - tile.data.navigationCoordinates)]
				.lineRenderer.enabled = false;

			nTile.areas[TileGrid.NeighbouringCoordinates.IndexOf(tile.data.navigationCoordinates - nTile.data.navigationCoordinates)]
				.lineRenderer.enabled = !add;
		}
	}
}
