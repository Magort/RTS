using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    public static Dictionary<Resource, int> Resources = new()
    {
        {Resource.Wood, 0 },
		{Resource.Food, 0 },
		{Resource.Gold, 0 },
		{Resource.Essence, 0 }
	};
}
