using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileInfoPanel : MonoBehaviour
{
	public TextMeshProUGUI infoText;
	[TextArea] public string notDiscoveredText;

	public List<MapUnitSlot> mapUnitSlots;
	public MapUnitWindow mapUnitWindow;

	void Start()
	{
		gameObject.SetActive(false);
	}

	public void PopulatePanel(bool discovered)
	{
		if (ContextMenu.Instance.SelectedTile == null)
			return;

		DepopulatePresentMapUnits();
		gameObject.SetActive(true);

		if (!discovered)
		{
			infoText.text = notDiscoveredText;
			return;
		}

		PopulatePresentMapUnits();

		infoText.text = TileInformation();
	}

	public void PopulatePresentMapUnits()
	{
		if (ContextMenu.Instance.SelectedTile == null)
			return;

		for (int i = 0; i < ContextMenu.Instance.SelectedTile.data.units.Count; i++)
		{
			mapUnitSlots[i].PopulateSlot(ContextMenu.Instance.SelectedTile.data.units[i]);
		}
	}

	void DepopulatePresentMapUnits()
	{
		foreach (MapUnitSlot mapUnitSlot in mapUnitSlots)
		{
			mapUnitSlot.DepopulateSlot();
		}
	}

	string TileInformation()
	{
		string info = "";

		Dictionary<Resource, int> addedResources = new()
		{
			{Resource.Wood, 0 },
			{Resource.Food, 0 },
			{Resource.Gold, 0 },
			{Resource.Essence, 0 }
		};

		foreach (TileArea area in ContextMenu.Instance.SelectedTile.areas)
		{
			switch (area.data.type)
			{
				case TileArea.Type.EssenceSource:
					addedResources[Resource.Essence] += area.data.resourceAmount;
					break;
				case TileArea.Type.WoodSource:
					addedResources[Resource.Wood] += area.data.resourceAmount;
					break;
				case TileArea.Type.FoodSource:
					addedResources[Resource.Food] += area.data.resourceAmount;
					break;
				case TileArea.Type.GoldSource:
					addedResources[Resource.Gold] += area.data.resourceAmount;
					break;
				default:
					break;
			}
		}

		foreach (var addedResource in addedResources)
		{
			if (addedResource.Value > 0)
			{
				info += "<sprite=" + IconIDs.resourceToIconID[addedResource.Key] + ">: " + addedResource.Value + "\n";
			}
		}

		if (info.Length == 0)
		{
			info = "There are no Resources here, but worry not, sir! It's an execelent spot for our Buildings!";
		}

		return info;
	}

	public void SelectAllUnits()
	{
		foreach (var slot in mapUnitSlots)
		{
			if (slot.gameObject.activeSelf)
				slot.OnClick();
		}

		AudioManager.Instance.Play(Sound.Name.Click);
	}

	public void ShowMapUnitWindow()
	{
		mapUnitWindow.PopulateWindow();

		AudioManager.Instance.Play(Sound.Name.Click);
	}
}
