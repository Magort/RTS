using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public (int, int) coordinates;
	public List<TileArea> areas;
    public Dictionary<TileArea.Type, GameObject> typeToDecoration = new();
	public void InitializeTile((int, int) coordinates)
    {
        this.coordinates = coordinates;
        RollAreas();
        SpawnDecorations();
    }

    public void TransformIntoTile(Tile tile)
    {
        areas = tile.areas;
        ClearDecorations();
        SpawnDecorations();
    }

    public void RollAreas()
    {
        List<TileArea.Type> rolledAreas = new() { TileArea.Type.Empty};

        for(int i = 1; i < areas.Count; i++)
        {
            var roll = Random.Range(0, 100);
            var type = (TileArea.Type)TileArea.TypeChances.FindIndex(chance => chance >= roll);

            if (GetAreaTypeCount(rolledAreas, type) < TileArea.MaxSameType)
                rolledAreas.Add(type);
            else
                rolledAreas.Add(TileArea.Type.Empty);
        }

		var sortedTypes = rolledAreas.OrderBy(o => (int)o).ToList();

        for(int i = 0; i < sortedTypes.Count; i++)
        {
            areas[i].type = sortedTypes[i];
        }
	}

	public int GetAreaTypeCount(List<TileArea.Type> types, TileArea.Type searchedType)
	{
		int count = 0;
		foreach(TileArea.Type type in types)
        {
            if(type == searchedType)
                count++;
        }

        return count;
	}

	private void SpawnDecorations()
    {
        foreach(TileArea area in areas)
        {
            area.ShowDecorations();
        }

        transform.Rotate(0, Random.Range(0, 6) * 60, 0);
    }
	private void ClearDecorations()
	{
        foreach(TileArea area in areas)
        {
            area.HideDecorations();
        }
	}
}
