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
