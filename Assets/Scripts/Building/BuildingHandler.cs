using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class BuildingHandler : MonoBehaviour
{
    public static BuildingHandler Instance;
    public List<BuildingSlot> buildingSlots;
    public string buildingText;
    public GameObject panel;

    private void Start()
    {
        Instance = this;
    }

    public void PopulateBuildingsList(bool discovered)
    {
        if (TileGrid.IsNextToPlyerKingdom(ContextMenu.Instance.SelectedTile)
            && ContextMenu.Instance.SelectedTile.affiliation != Affiliation.Player)
        { 
            panel.SetActive(false);
            return;
        }

		panel.SetActive(discovered);

		ClearBuildingsList();

		if (ContextMenu.Instance.SelectedTile.areas.Find(area => area.type == TileArea.Type.Empty) == null)
            return;

		foreach (BuildingSlot slot in buildingSlots)
        {
            if(slot.unlocked && slot.building.ValidPlacement())
            {
                slot.gameObject.SetActive(true);
            }
        }
    }

    public void SwitchBuildingLock(Building.Code building, bool state)
    {
        buildingSlots.Find(slot => slot.building.code == building).unlocked = state;
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

        if (ContextMenu.Instance.SelectedTile.affiliation == Affiliation.Enemy)
            return false;

        if(building.isUpgrade)
            Upgrade(building);
        else
            Build(building);

        SubstractResources(building);
        return true;
    }

    void Upgrade(Building building)
    {
		var currentBuildingArea = ContextMenu.Instance.SelectedTile.areas
            .Where(area => area.type == TileArea.Type.Building).ToList()
            .Find(area => area.building == building.requirements.requiredBuilding);

		currentBuildingArea.building = building.code;
		StartCoroutine(DelayUpgrade(building, currentBuildingArea, ContextMenu.Instance.SelectedTile));
	}

    void Build(Building building)
    {
        var freeArea = ContextMenu.Instance.SelectedTile.areas.Find(area => area.type == TileArea.Type.Empty);
        freeArea.type = TileArea.Type.Building;

        StartCoroutine(DelayBuild(building, freeArea, ContextMenu.Instance.SelectedTile));
		freeArea.building = building.code;
	}

	IEnumerator DelayUpgrade(Building building, TileArea tileArea, Tile tile)
	{
		ProgressBarManager.Instance.GetProgressBar().ShowProgress(tileArea.buildingSlot.transform, building.buildingTime, buildingText);

		yield return new WaitForSeconds(building.buildingTime);

        tileArea.RemoveBuilding();

		Instantiate(building.gameObject, tileArea.buildingSlot.transform.position, Quaternion.identity, tileArea.buildingSlot.transform)
			.GetComponent<Building>().OnBuildingComplete(tile, tileArea);

		tile.ChangeAffiliation(Affiliation.Player);
        tileArea.type = TileArea.Type.Building;
	}

	IEnumerator DelayBuild(Building building, TileArea tileArea, Tile tile)
    {
        ProgressBarManager.Instance.GetProgressBar().ShowProgress(tileArea.buildingSlot.transform, building.buildingTime, buildingText);

        yield return new WaitForSeconds(building.buildingTime);

		Instantiate(building.gameObject, tileArea.buildingSlot.transform.position, Quaternion.identity, tileArea.buildingSlot.transform)
	        .GetComponent<Building>().OnBuildingComplete(tile, tileArea);

		tile.ChangeAffiliation(Affiliation.Player);
	}

	void SubstractResources(Building building)
    {
        foreach(var requirement in building.requirements.resourceRequirements)
        {
            GameState.AddResource(requirement.resource, -requirement.amount);
        }
    }
}
