using System.Collections.Generic;

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

		description += "every <b>";

		foreach (var time in productionSpeeds)
		{
			description += time + "/";
		}

		description = description.Remove(description.Length - 1);

		description += "</b> seconds. Unlocks new buildings. Grants 1 more Scout. Increases max army size by 1.";

		return description;
	}

	public override void SpecialOnBuild(Tile tile, TileArea area)
	{
		base.SpecialOnBuild(tile, area);

		foreach (var building in buildingsToUnlock)
		{
			BuildingHandler.Instance.SwitchBuildingLock(building, true);
		}

		GameState.AddScouts(1);
		MapUnit.playerArmylimit++;
	}
}
