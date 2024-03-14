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
    public List<Building.Code> availableBuildings;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadAvailableBuildingsList(List<Building.Code> buildings)
    {
        availableBuildings = buildings;
    }

    public void PopulateBuildingsList(bool discovered)
    {
        if (TileGrid.IsNextToPlyerKingdom(ContextMenu.Instance.SelectedTile)
            && ContextMenu.Instance.SelectedTile.data.affiliation != Affiliation.Player)
        {
            panel.SetActive(false);
            return;
        }

		panel.SetActive(discovered);

		ClearBuildingsList();

		if (ContextMenu.Instance.SelectedTile.areas.Find(area => area.data.type == TileArea.Type.Empty) == null)
            return;

        for (int i = 0; i < availableBuildings.Count; i++)
        {
            var building = GameManager.Instance.BuildingsList.Find(building => building.code == availableBuildings[i]);

            if (building.ValidPlacement())
                buildingSlots[i].PopulateButton(building);
        }
    }

    public void SwitchBuildingLock(Building.Code building, bool available)
    {
        if(available)
            availableBuildings.Add(building);
        else
            availableBuildings.Remove(building);
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

        if (ContextMenu.Instance.SelectedTile.data.affiliation == Affiliation.Enemy)
            return false;

        if(building.isUpgrade)
            Upgrade(building);
        else
            Build(building);

        SubstractResources(building);
        return true;
    }

    public void LoadBuilding(TileArea tileArea, bool enable)
    {
        Instantiate(GameManager.Instance.BuildingsList.Find(building => building.code == tileArea.data.building)
            ,tileArea.buildingSlot.transform.position, Quaternion.identity, tileArea.buildingSlot.transform)
            .GetComponent<Building>().enabled = enable;
    }

    void Upgrade(Building building)
    {
		var currentBuildingArea = ContextMenu.Instance.SelectedTile.areas
            .Where(area => area.data.type == TileArea.Type.Building).ToList()
            .Find(area => area.data.building == building.requirements.requiredBuilding);

		currentBuildingArea.data.building = building.code;
		StartCoroutine(DelayUpgrade(building, currentBuildingArea, ContextMenu.Instance.SelectedTile));
	}

    void Build(Building building)
    {
        var freeArea = ContextMenu.Instance.SelectedTile.areas.Find(area => area.data.type == TileArea.Type.Empty);
        freeArea.data.type = TileArea.Type.Building;

        StartCoroutine(DelayBuild(building, freeArea, ContextMenu.Instance.SelectedTile));
		freeArea.data.building = building.code;
	}

	IEnumerator DelayUpgrade(Building building, TileArea tileArea, Tile tile)
	{
		ProgressBarManager.Instance.GetProgressBar()
            .ShowProgress(tileArea.buildingSlot.transform, building.buildingTime, buildingText, TileArea.affiliationToColor[Affiliation.Player]);

		yield return new WaitForSeconds(building.buildingTime);

        tileArea.RemoveBuilding();

		Instantiate(building.gameObject, tileArea.buildingSlot.transform.position, Quaternion.identity, tileArea.buildingSlot.transform)
			.GetComponent<Building>().OnBuildingComplete(tile, tileArea);

		tile.ChangeAffiliation(Affiliation.Player);
        tileArea.data.type = TileArea.Type.Building;
	}

	IEnumerator DelayBuild(Building building, TileArea tileArea, Tile tile)
    {
        ProgressBarManager.Instance.GetProgressBar()
            .ShowProgress(tileArea.buildingSlot.transform, building.buildingTime, buildingText, TileArea.affiliationToColor[Affiliation.Player]);

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
