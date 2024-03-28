using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : ScriptableObject
{
    public Vector3 cameraStartingPos;
    public List<MissionObjective> missionObjects;
    public List<Building.Requirements.ResourceRequirement> startingResources;
    public List<Building.Code> availableBuildings;
    public List<TileData> tiles = new();
}
