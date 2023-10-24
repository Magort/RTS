using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : ResourceProducer
{
	public List<Code> buildingsToUnlock;
    public override string Description()
    {
		string description = "Produces ";
		foreach (var resource in resourcesToProduce)
		{
			description += "1<sprite="
			   + IconIDs.resourceToIconID[resource] + "> ";
		}

		description += "every <b>" + productionSpeed + "</b> seconds.";

		description += " Unlocks new buildings.";

		return description;
	}

	public override void SpecialOnBuild(Tile tile, TileArea area)
	{
		base.SpecialOnBuild(tile, area);

		foreach(var building in buildingsToUnlock)
		{
			BuildingHandler.Instance.SwitchBuildingLock(building, true);
		}
	}
}
