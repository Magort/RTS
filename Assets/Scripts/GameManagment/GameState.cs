using System.Collections.Generic;

public static class GameState
{
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

	public static int ScoutsAvailable = 2;

	public static void AddResource(Resource resource, int amount)
	{
		Resources[resource] += amount;
		ResourcesPanel.Instance.UpdateDisplay();
	}
	public static void AddResourceGrowth(Resource resource, float amount)
	{
		ResourcesGrowth[resource] += amount;
		ResourcesPanel.Instance.UpdateDisplay();
	}
}
