using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileGrid
{
    public static List<Tile> Tiles = new List<Tile>();

    public static void AddTile(Tile tile)
    {
        Tiles.Add(tile);
    }

}
