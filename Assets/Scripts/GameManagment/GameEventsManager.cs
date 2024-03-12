using UnityEngine.Events;
using UnityEngine;

public static class GameEventsManager
{
    public static UnityEvent<Building.Code> BuildingCompleted;
    public static UnityEvent<Building.Code> BuildingRemoved;
    public static UnityEvent<Vector3Int> TileControlled;
    public static UnityEvent<Vector3Int> TileLostControll;
    public static UnityEvent<Vector3Int> TileDiscovered;
}
