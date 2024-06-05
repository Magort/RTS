using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapUnit
{
	public string customName = "Army";
	public List<Unit> units = new();
	public Affiliation affiliation;
	public static float playerArmylimit = 5;
	[HideInInspector] public ProgressBar movementProgressBar = null;
	[HideInInspector] public Tile currentTile;
	[HideInInspector] public List<Tile> path = new();

	public int GetSpeedSum()
	{
		int sum = 0;

		foreach (Unit unit in units)
		{
			sum += unit.speed;
		}

		return sum;
	}

	public int GetHealthSum()
	{
		int sum = 0;

		foreach (Unit unit in units)
		{
			sum += unit.health;
		}

		return sum;
	}

	public void SetCustomName(string newName)
	{
		customName = newName;
	}

	public bool CanAddToArmy()
	{
		return units.Count < playerArmylimit;
	}

	public void AddToArmy(Unit unit)
	{
		units.Add(unit);
	}

	public void KillUnit(Unit unit)
	{
		if (affiliation == Affiliation.Player)
		{
			foreach (var requirement in unit.upkeepCost)
			{
				UnitUpkeepHandler.Instance.RemoveUpkeep(requirement.amount, requirement.resource);
			}
		}

		RemoveFromArmy(unit);
	}

	public void KillRandomUnit()
	{
		var unit = units[Random.Range(0, units.Count)];

		Debug.Log(customName);

		if (affiliation == Affiliation.Player)
		{
			foreach (var requirement in unit.upkeepCost)
			{
				UnitUpkeepHandler.Instance.RemoveUpkeep(requirement.amount, requirement.resource);
			}
		}

		RemoveFromArmy(unit);
	}

	public void RemoveFromArmy(Unit unit)
	{
		units.Remove(unit);

		if (units.Count == 0)
		{
			currentTile.RemoveUnit(this);
		}
	}
}
