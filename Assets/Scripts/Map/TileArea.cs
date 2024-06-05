using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileArea : MonoBehaviour
{
	public static List<int> TypeChances = new() { 75, 85, 95, 98, 100 };
	public static Dictionary<Type, int> MaxSameType = new()
	{
		{Type.Empty, 6 },
		{Type.WoodSource, 3 },
		{Type.FoodSource, 3 },
		{Type.GoldSource, 2 },
		{Type.EssenceSource, 1 }
	};
	public static Dictionary<Type, int> ResourceStartingAmount = new()
	{
		{Type.Empty, 0 },
		{Type.WoodSource, 150 },
		{Type.FoodSource, 75 },
		{Type.GoldSource, 25 },
		{Type.EssenceSource, 10000 }
	};
	public static Dictionary<Affiliation, Color> affiliationToColor = new()
	{
		{ Affiliation.Player, Color.blue },
		{ Affiliation.Enemy, Color.red },
		{ Affiliation.Neutral, Color.green }
	};

	public TileAreaData data = new();

	public GameObject woodDecorations;
	public GameObject foodDecorations;
	public GameObject goldDecorations;
	public GameObject essenceDecorations;
	public GameObject buildingSlot;
	public LineRenderer lineRenderer;

	Dictionary<Type, GameObject> typeToDecoration;

	public void InitDictionary()
	{
		if (typeToDecoration != null)
			return;

		typeToDecoration = new()
		{
			{ Type.WoodSource, woodDecorations },
			{ Type.FoodSource, foodDecorations },
			{ Type.GoldSource, goldDecorations },
			{ Type.EssenceSource, essenceDecorations },
			{ Type.Building, buildingSlot }
		};
	}

	public void DepleteResources()
	{
		HideDecorations();
		data.type = Type.Empty;
	}

	public void RemoveBuilding()
	{
		Destroy(buildingSlot.transform.GetChild(0).gameObject);
		data.type = Type.Empty;
		GameEventsManager.BuildingRemoved.Invoke(data.building);

	}

	public void ShowDecorations()
	{
		if (data.type != Type.Empty)
		{
			typeToDecoration[data.type].SetActive(true);
		}
	}

	public void HideDecorations()
	{
		if (data.type != Type.Empty)
		{
			typeToDecoration[data.type].SetActive(false);
		}
	}

	public enum Type
	{
		Empty,
		WoodSource,
		FoodSource,
		GoldSource,
		EssenceSource,
		Building
	}
}
