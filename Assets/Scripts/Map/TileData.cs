using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
	public bool discovered;
	public bool neighbour;
	public bool beingScouted;
	public List<MapUnit> units;
	public Affiliation affiliation;
	public Biome biome;
	public Vector3Int navigationCoordinates;
	public Vector3 spaceCoordinates;
	public List<TileAreaData> areas;
}
