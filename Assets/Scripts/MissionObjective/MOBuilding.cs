public class MOBuilding : MissionObjective
{
	public Building.Code requiredBuilding;

	public override bool ConditionsMet()
	{
		return GameState.GetBuildingAmount(requiredBuilding) >= quantityToPass;
	}

	public override void Innit()
	{
		GameEventsManager.BuildingCompleted.AddListener(CheckNewBuilding);
	}

	void CheckNewBuilding(Building.Code newBuilding)
	{
		if(newBuilding == requiredBuilding)
		{
			ProgressQuantity();
		}
	}

	public override void Clear()
	{
		GameEventsManager.BuildingCompleted.RemoveListener(CheckNewBuilding);
	}
}
