using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : ScriptableObject
{
    public List<Building.Requirements.ResourceRequirement> startingResources;
    public List<Building> availableBuildings;
    public List<TileData> tiles = new();
}
