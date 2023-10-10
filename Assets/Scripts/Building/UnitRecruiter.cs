using UnityEngine;

public class UnitRecruiter : Building
{
    public Unit unit;

	public override void SpecialOnBuild(Tile tile, TileArea area)
    {
        tile.unitRecruiters.Add(this);
        ContextMenu.Instance.unitRecruitmentPanel.PopulateUnitSlots();
    }

    public void Recruit()
    {
        AddUnit();
        SubstarctResources();
        StartUpkeep();
    }

    public void AddUnit()
    {
        var alliedMapUnits = builtOn.units.FindAll(unit => unit.affiliation == builtOn.affiliation);

		MapUnit mapUnitWithFreeSlots = null;

		if (alliedMapUnits.Count > 0)
        {
            foreach(var unit in alliedMapUnits)
            {
                if(unit.CanAddToArmy())
                {
                    mapUnitWithFreeSlots = unit;
                    break;
                }
            }
        }

        if(mapUnitWithFreeSlots != null)
        {
            mapUnitWithFreeSlots.AddToArmy(unit);
            return;
        }

        mapUnitWithFreeSlots = new();
        mapUnitWithFreeSlots.affiliation = builtOn.affiliation;

		mapUnitWithFreeSlots.AddToArmy(unit);

        builtOn.AddUnit(mapUnitWithFreeSlots);
	}

    public void SubstarctResources()
    {
		foreach (var requirement in unit.recruitCost)
		{
			GameState.AddResource(requirement.resource, -requirement.amount);
		}
	}

    public void StartUpkeep()
    {
		foreach (var requirement in unit.upkeepCost)
		{
			GameState.AddResourceGrowth(requirement.resource, -requirement.amount);
		}
	}

    public bool CanAfford()
    {
		foreach (var resourceRequirement in unit.recruitCost)
		{
			if (GameState.Resources[resourceRequirement.resource] < resourceRequirement.amount)
				return false;
		}

		return true;
	}
}
