using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KindgomLine : MonoBehaviour
{
    public static KindgomLine Instance;
    

    private void Start()
    {
        Instance = this;
    }
    public void AddTileToBorder(Tile tile)
    {
        List<Tile> tilesToUpdate = TileGrid.GetNeighbouringTiles(tile)
            .Where(tile => tile.affiliation == Affiliation.Player).ToList();

        Debug.Log(tilesToUpdate.Count);

        foreach(TileArea area in tile.areas)
        {
            area.lineRenderer.enabled = true;
        }

        foreach(Tile nTile in tilesToUpdate)
        {
            tile.areas[TileGrid.NeighbouringCoordinates.IndexOf(nTile.coordinates - tile.coordinates)]
                .lineRenderer.enabled = false;

			nTile.areas[TileGrid.NeighbouringCoordinates.IndexOf(tile.coordinates - nTile.coordinates)]
				.lineRenderer.enabled = false;
		}
    }
}
