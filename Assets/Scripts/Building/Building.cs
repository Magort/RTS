using System.Collections.Generic;
using UnityEngine;


public abstract class Building : MonoBehaviour
{
    public string _name;
    [TextArea] public string description;
    public Tile builtOn;
	public Requirements requirements;

	public bool ValidPlacement()
	{
		if (!ContextMenu.Instance.SelectedTile.areas.Find(area => area.type == requirements.requiredArea))
			return false;

		return true;
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
	public struct Requirements
	{
		[System.Serializable]
		public struct ResourceRequirement
		{
			public Resource resource;
			public int amount;
		}
		public List<ResourceRequirement> resourceRequirements;
		public TileArea.Type requiredArea;
	}
}
