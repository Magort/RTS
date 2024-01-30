using System.Collections.Generic;
using UnityEngine;

public class MapUnitWindow : MonoBehaviour
{
    public static MapUnitWindow Instance;

    public MapUnitWindowUnitArea mapUnitArea1;
    public MapUnitWindowUnitArea mapUnitArea2;
    public MapUnitWindowMapUnitArea mapUnitsArea1;
    public MapUnitWindowMapUnitArea mapUnitsArea2;
    public GameObject blockingPanel;
    public List<MapUnitWindowDropArea> dropAreas;

    Dictionary<MapUnitWindowDropArea.AreaType, MapUnitWindowUnitArea> typeToArea = new();
    public Tile selectedTile;

	private void Awake()
    {
        typeToArea = new()
        {
            { MapUnitWindowDropArea.AreaType.MapUnitArea1, mapUnitArea1 },
            { MapUnitWindowDropArea.AreaType.MapUnitArea2, mapUnitArea2 }
		};

		Instance = this;
	}

    private void OnDisable()
    {
		mapUnitsArea1.ClearSlots();
		mapUnitsArea2.ClearSlots();
        mapUnitArea1.ClearArea();
        mapUnitArea2.ClearArea();

		List<MapUnit> toRemove = new();

        foreach(var mapUnit in selectedTile.data.units)
        {
            if(mapUnit.units.Count == 0)
                toRemove.Add(mapUnit);
        }

        toRemove.ForEach(unit => selectedTile.data.units.Remove(unit));
        selectedTile = null;
		blockingPanel.SetActive(false);

		GameManager.SwitchPauseState(false);
	}

    public void PopulateWindow()
    {
        foreach(var mapUnit in ContextMenu.Instance.SelectedTile.data.units)
        {
            if (mapUnit.affiliation == Affiliation.Player)
                mapUnitsArea1.ShowSlot(mapUnit);
            else
                mapUnitsArea2.ShowSlot(mapUnit);
        }

		selectedTile = ContextMenu.Instance.SelectedTile;
		ContextMenu.Instance.CloseAll();
        GameManager.SwitchPauseState(true);

        blockingPanel.SetActive(true);
        gameObject.SetActive(true);
    }

    public void SelectMapUnit(MapUnitWindowDropArea.AreaType destination, MapUnit mapUnit)
    {
        typeToArea[destination].PopulateArea(mapUnit);
    }

    public void MoveUnit(MapUnitWindowDropArea.AreaType destination, Unit unit)
    {
        if (typeToArea[destination].mapUnit.affiliation != Affiliation.Player)
            return;

        if (typeToArea[destination].mapUnit.units.Count >= MapUnit.playerArmylimit)
            return;

		typeToArea[destination].mapUnit.AddToArmy(unit);
		GetOtherUnitArea(typeToArea[destination]).mapUnit.RemoveFromArmy(unit);

        mapUnitArea1.PopulateArea(mapUnitArea1.mapUnit);
        mapUnitArea2.PopulateArea(mapUnitArea2.mapUnit);
    }

	MapUnitWindowUnitArea GetOtherUnitArea(MapUnitWindowUnitArea unitArea)
    {
        if (unitArea == mapUnitArea1)
            return mapUnitArea2;
        else
            return mapUnitArea1;
	}

    public void ToggleDropAreas(bool state)
    {
        dropAreas.ForEach(area => area.gameObject.SetActive(state));
    }
}
