using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameState
{
	public static bool TapLocked;
	public static bool DragLocked;

	public static Dictionary<Resource, int> Resources = new()
	{
		{Resource.Wood, 0 },
		{Resource.Food, 0 },
		{Resource.Gold, 0 },
		{Resource.Essence, 0 }
	};

	public static Dictionary<Resource, float> ResourcesGrowth = new()
	{
		{Resource.Wood, 0 },
		{Resource.Food, 0 },
		{Resource.Gold, 0 },
		{Resource.Essence, 0 }
	};

	public static int MaxScouts = 0;
	public static int ScoutsAvailable = 0;

	public static List<Building.Code> BuildingsBuilt = new();
	public static List<Unit.Code> UnitsControlled = new();

	public static void AddScouts(int amount)
	{
		MaxScouts += amount;
		ScoutsAvailable += amount;
	}

	public static void AddResource(Resource resource, float amount)
	{
		int intAmount = Mathf.RoundToInt(amount);
		Resources[resource] += intAmount;
		ResourcesPanel.Instance.UpdateDisplay();
	}
	public static void AddResourceGrowth(Resource resource, float amount)
	{
		ResourcesGrowth[resource] += amount;
		ResourcesPanel.Instance.UpdateDisplay();
	}

	public static void AddBuilding(Building.Code building)
	{
		BuildingsBuilt.Add(building);
	}
	public static void RemoveBuilding(Building.Code building)
	{
		BuildingsBuilt.Remove(building);
	}

	public static int GetBuildingAmount(Building.Code building)
	{
		return BuildingsBuilt.Where(entry => entry == building).Count();
	}

	public static int GetUnitAmount(Unit.Code unit)
	{
		return UnitsControlled.Where(entry => entry == unit).Count();
	}
}
