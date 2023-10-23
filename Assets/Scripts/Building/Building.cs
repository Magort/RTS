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

	public void OnBuildingComplete(Tile tile, TileArea area)
	{
		builtOn = tile;
		SpecialOnBuild(tile, area);
	}
	public abstract void SpecialOnBuild(Tile tile, TileArea area);
	public abstract string Description();
	public enum Code
	{
		Woodcutters,
		Gatherers,
		Mine,
		Researchers,
		Fighters
	}

	public string RequirementsToString()
	{
		string requirementsString = "<b>Building Cost:\n</b>";

		foreach(Requirements.ResourceRequirement requirement in requirements.resourceRequirements)
		{
			requirementsString += "<sprite=" + IconIDs.resourceToIconID[requirement.resource] + "> " + requirement.amount + " ";
		}

		return requirementsString;
	}

	public bool ValidPlacement()
	{
		return ContextMenu.Instance.SelectedTile.areas.Find(area => area.type == requirements.requiredArea)
				&& ContextMenu.Instance.SelectedTile.areas.Where(area => area.type == TileArea.Type.Building)
					.ToList().Find(area => area.buildingsBuilt.Contains(code)) == null;
	}

	public bool SufficientResources()
	{
		foreach(var resourceRequirement in requirements.resourceRequirements)
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
			public int amount;
		}
		public List<ResourceRequirement> resourceRequirements;
		public TileArea.Type requiredArea;
	}
}
