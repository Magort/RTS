using UnityEngine;
using UnityEngine.Events;

public static class GameEventsManager
{
	public static UnityEvent<Building.Code> BuildingCompleted = new();
	public static UnityEvent<Building.Code> BuildingRemoved = new();
	public static UnityEvent<Vector3Int> TileControlled = new();
	public static UnityEvent<Vector3Int> TileLostControll = new();
	public static UnityEvent<Vector3Int> TileDiscovered = new();
	public static UnityEvent<Vector3Int> TileSelected = new();
	public static UnityEvent<MapUnit, Vector3Int> UnitMoveOrder = new();
	public static UnityEvent<MapUnit> MapUnitSelected = new();
	public static UnityEvent<Unit.Code> UnitRecruited = new();
	public static UnityEvent<Affiliation> ActionsRolled = new();
	public static UnityEvent RoundEnded = new();
}
