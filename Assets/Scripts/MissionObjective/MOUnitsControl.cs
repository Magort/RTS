using UnityEngine;

[CreateAssetMenu(fileName = "MissionObjective", menuName = "MissionObjectives/UnitControl", order = 5)]
public class MOUnitsControl : MissionObjective
{
	public Unit.Code unitByCode;

	public override void Clear()
	{
		GameEventsManager.UnitRecruited.RemoveListener(CheckUnit);
	}

	public override bool ConditionsMet()
	{
		return GameState.GetUnitAmount(unitByCode) >= quantityToPass;
	}

	public override void Innit()
	{
		GameEventsManager.UnitRecruited.AddListener(CheckUnit);
	}

	void CheckUnit(Unit.Code unit)
	{
		if(unit == unitByCode)
		{
			ProgressQuantity();
		}
	}
}
