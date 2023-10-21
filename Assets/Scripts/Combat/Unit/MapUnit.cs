using System.Collections.Generic;

[System.Serializable]
public class MapUnit
{
    public string customName = "Army";
    public List<Unit> units = new();
    public Affiliation affiliation;
    public static float armylimit = 10;
    public Tile currentTile;
    public List<Tile> path = new();

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
        return units.Count < armylimit;
    }

    public void AddToArmy(Unit unit)
    {
        units.Add(unit);
    }
}
