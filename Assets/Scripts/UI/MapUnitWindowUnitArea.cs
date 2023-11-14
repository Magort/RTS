using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapUnitWindowUnitArea : MonoBehaviour
{
    public TextMeshProUGUI nameBox;
    public List<MapUnitWindowSlot> slots;
    [HideInInspector] public MapUnit mapUnit = null;

    public void PopulateArea(MapUnit mapUnit)
    {
        this.mapUnit = mapUnit;
        ClearSlots();

        nameBox.text = mapUnit.customName;

        for(int i = 0; i < mapUnit.units.Count; i++)
        {
            slots[i].PopulateSlot(mapUnit.units[i]);
            slots[i].SwitchDraggable(mapUnit.affiliation == Affiliation.Player);
        }

		gameObject.SetActive(true);
	}

    public void ClearArea()
    {
        mapUnit = null;
        ClearSlots();
        nameBox.text = "";
    }

    public void ClearSlots()
    {
        slots.ForEach(slot => slot.ClearSlot());
    }

    public void ChangeUnitName(string newName)
    {
        mapUnit.SetCustomName(newName);
        nameBox.text = newName;
    }
}
