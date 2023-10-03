using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public static BuildingHandler Instance;
    public List<BuildingSlot> buildingSlots;

    private void Start()
    {
        Instance = this;
    }

    public void PopulateBuildingsList(bool discovered)
    {
		gameObject.SetActive(discovered);

		if (!ContextMenu.Instance.SelectedTile.areas.Find(area => area.type == TileArea.Type.Empty))
            return;

		ClearBuildingsList();

		foreach (BuildingSlot slot in buildingSlots)
        {
            if(slot.unlocked && slot.building.ValidPlacement())
            {
                slot.gameObject.SetActive(true);
            }
        }
    }

    public void ClearBuildingsList()
    {
		foreach (BuildingSlot slot in buildingSlots)
		{
		    slot.gameObject.SetActive(false);
		}
	}

    public bool TryBuild(Building building)
    {
        if(!building.SufficientResources())
            return false;

        Build(building);
        ExtractResources(building);
        return true;
    }

    void Build(Building building)
    {
        var freeArea = ContextMenu.Instance.SelectedTile.areas.Find(area => area.type == TileArea.Type.Empty);
        freeArea.type = TileArea.Type.Building;
        Instantiate(building.gameObject, freeArea.buildingSlot.transform.position, Quaternion.identity, freeArea.buildingSlot.transform)
            .GetComponent<Building>().builtOn = ContextMenu.Instance.SelectedTile;
    }

    void ExtractResources(Building building)
    {
        foreach(var requirement in building.requirements.resourceRequirements)
        {
            GameState.AddResource(requirement.resource, -requirement.amount);
        }
    }
}
