using UnityEngine.Events;
using UnityEngine;

public static class GameEventsManager
{
    public static UnityEvent<Building.Code> BuildingCompleted = new();
    public static UnityEvent<Building.Code> BuildingRemoved = new();
    public static UnityEvent<Vector3Int> TileControlled = new();
    public static UnityEvent<Vector3Int> TileLostControll = new();
    public static UnityEvent<Vector3Int> TileDiscovered = new();
}
