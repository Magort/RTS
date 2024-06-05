using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
	public string _name;
	public int buildingTime;
	public Tile builtOn;
	public Requirements requirements;
	public Code code;
	public bool isUpgrade;
	public Sprite icon;

	public void OnBuildingComplete(Tile tile, TileArea area)
	{
		builtOn = tile;
		GameEventsManager.BuildingCompleted.Invoke(code);
		SpecialOnBuild(tile, area);
		GameEventsManager.BuildingCompleted.Invoke(code);
	}
	public abstract void SpecialOnBuild(Tile tile, TileArea area);
	public abstract string Description();
	public enum Code
	{
		Woodcutters,
		Gatherers,
		Mine,
		Researchers,
		Fighters,
		MainBase,
		MainBase2,
		MainBase3,
		Druids,
		Skirmishers,
		Archers,
		Zephyrs,
		Traders,
		Farmers,
		Watchmen,
		GroveTenders,
		Elementalists,
		SoulMasters,
		Altar
	}

	public string RequirementsToString()
	{
		string requirementsString = "<b>Building Cost:\n</b>";

		foreach (Requirements.ResourceRequirement requirement in requirements.resourceRequirements)
		{
			requirementsString += "<sprite=" + IconIDs.resourceToIconID[requirement.resource] + "> " + requirement.amount + " ";
		}

		if (isUpgrade)
		{
			requirementsString += "\n<b>Upgrade of</b>: " + requirements.requiredBuilding.ToString();
		}

		return requirementsString;
	}

	public bool ValidPlacement()
	{
		if (isUpgrade)
			return ContextMenu.Instance.SelectedTile.areas
				.Where(area => area.data.type == TileArea.Type.Building).ToList()
				.Find(area => area.data.building == requirements.requiredBuilding) != null;

		return ContextMenu.Instance.SelectedTile.areas.Find(area => area.data.type == requirements.requiredArea)
				&& ContextMenu.Instance.SelectedTile.areas.Where(area => area.data.type == TileArea.Type.Building)
					.ToList().Find(area => area.data.building == code) == null;
	}

	public bool SufficientResources()
	{
		foreach (var resourceRequirement in requirements.resourceRequirements)
		{
			if (GameState.Resources[resourceRequirement.resource] < resourceRequirement.amount)
				return false;
		}

		return true;
	}

	[System.Serializable]
	public class Requirements
	{
		[System.Serializable]
		public class ResourceRequirement
		{
			public Resource resource;
			public float amount;
		}
		public List<ResourceRequirement> resourceRequirements;
		public TileArea.Type requiredArea;
		public Code requiredBuilding;
	}
}
