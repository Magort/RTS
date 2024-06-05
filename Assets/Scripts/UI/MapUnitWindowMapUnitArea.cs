using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapUnitWindowMapUnitArea : MonoBehaviour
{
	public List<MapUnitWindowSlot> slots;

	public void ShowSlot(MapUnit mapUnit)
	{
		slots.First(slot => !slot.gameObject.activeSelf).PopulateSlot(mapUnit);
	}

	public void ClearSlots()
	{
		slots.ForEach(slot => slot.gameObject.SetActive(false));
	}
}
