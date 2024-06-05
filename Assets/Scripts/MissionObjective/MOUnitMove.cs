using UnityEngine;

[CreateAssetMenu(fileName = "MissionObjective", menuName = "MissionObjectives/UnitMove", order = 8)]
public class MOUnitMove : MissionObjective
{
	public Vector3Int destinationTile;
	public Vector3Int unitTile;
	public int unitIndex;

	public bool showCombatTutorial;

	MapUnit requiredUnit;

	public override void Clear()
	{
		TileGrid.Tiles.Find(tile => tile.data.navigationCoordinates == destinationTile).SwitchParticles();
		GameEventsManager.UnitMoveOrder.RemoveListener(CheckTile);
	}

	public override bool ConditionsMet()
	{
		return true;
	}

	public override void Innit()
	{
		requiredUnit = TileGrid.GetTile(unitTile).data.units[unitIndex];
		if (showCombatTutorial)
		{
			CombatPanel.Instance.combatTutorial.SetActive(true);
		}
		TileGrid.Tiles.Find(tile => tile.data.navigationCoordinates == destinationTile).SwitchParticles();
		GameEventsManager.UnitMoveOrder.AddListener(CheckTile);
	}

	void CheckTile(MapUnit unit, Vector3Int tileCoords)
	{
		if (unit == requiredUnit && tileCoords == destinationTile)
			ProgressQuantity();
	}
}
