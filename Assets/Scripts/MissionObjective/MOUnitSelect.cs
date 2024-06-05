using UnityEngine;

[CreateAssetMenu(fileName = "MissionObjective", menuName = "MissionObjectives/UnitSelect", order = 7)]
public class MOUnitSelect : MissionObjective
{
	public Vector3Int unitTile;
	public int unitIndex;

	MapUnit requiredUnit;

	public override void Clear()
	{
		GameEventsManager.MapUnitSelected.RemoveListener(CheckArmy);
	}

	public override bool ConditionsMet()
	{
		return UnitMovementHandler.Instance.selectedUnits.Contains(requiredUnit);
	}

	public override void Innit()
	{
		requiredUnit = TileGrid.GetTile(unitTile).data.units[unitIndex];
		GameEventsManager.MapUnitSelected.AddListener(CheckArmy);
	}

	void CheckArmy(MapUnit unit)
	{
		if (unit == requiredUnit)
			ProgressQuantity();
	}
}
