using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [Header("Data")]
    public bool discovered;
    public bool neighbour;
    public Vector2 debugCoordinates;
    public (int, int) coordinates;
	[Header("Light")]
    public Light lighting;
    public float maxIntensity;
    [Header("Tile Areas")]
    public GameObject container;
	public List<TileArea> areas;
    public Dictionary<TileArea.Type, GameObject> typeToDecoration = new();

    public void InitializeTile((int, int) coordinates)
    {
        this.coordinates = coordinates;
#if UNITY_EDITOR
        debugCoordinates = new Vector2(coordinates.Item1, coordinates.Item2);
#endif
        container.transform.Rotate(0, Random.Range(0, 6) * 60, 0);
		RollAreas();
    }
    public void TransformIntoTile(Tile tile)
    {
        for(int i = 0; i < areas.Count; i++)
        {
            areas[i].type = tile.areas[i].type;
			areas[i].resourceAmount = tile.areas[i].resourceAmount;
		}
    }
    public void RollAreas()
    {
        List<TileArea.Type> rolledAreas = new() { TileArea.Type.Empty};

        for(int i = 1; i < areas.Count; i++)
        {
            var roll = Random.Range(0, 100);
            var type = (TileArea.Type)TileArea.TypeChances.FindIndex(chance => chance >= roll);

            if (GetAreaTypeCount(rolledAreas, type) < TileArea.MaxSameType[type])
                rolledAreas.Add(type);
            else
                rolledAreas.Add(TileArea.Type.Empty);
        }

		var sortedTypes = rolledAreas.OrderBy(o => (int)o).ToList();

        for(int i = 0; i < sortedTypes.Count; i++)
        {
            areas[i].type = sortedTypes[i];
            areas[i].resourceAmount = TileArea.ResourceStartingAmount[sortedTypes[i]];
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
	private void ShowDecorations()
    {
        foreach(TileArea area in areas)
        {
            area.ShowDecorations();
        }
    }
	private void ClearDecorations()
	{
        Debug.Log(coordinates);

		foreach (TileArea area in areas)
        {
            area.HideDecorations();
        }
	}

    public void BecomeNeighbour()
    {
        StartCoroutine(BecomeNeighbourCoroutine());
    }
	IEnumerator BecomeNeighbourCoroutine()
	{
		float intensityGrowth = maxIntensity/100;
        WaitForSeconds waiter = new(0.01f);

		while (lighting.intensity < maxIntensity/5)
		{
			lighting.intensity += intensityGrowth;
			yield return waiter;
		}

		neighbour = true;
	}

	public void Reveal()
    {
		StartCoroutine(RevealCoroutine());
	}

    IEnumerator RevealCoroutine()
    {
        float intensityGrowth = maxIntensity/100;
        WaitForSeconds waiter = new(0.01f);

		while(lighting.intensity < maxIntensity)
        {
            lighting.intensity += intensityGrowth;
            yield return waiter;
        }

		discovered = true;

		foreach (Tile tile in TileGrid.GetNeighbouringTiles(coordinates))
		{
            if(!tile.neighbour)
			    tile.BecomeNeighbour();
		}

        ShowDecorations();
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            return;

        if(!neighbour)
        {
			ContextMenu.Instance.CloseAll();
			return;
        }

        ContextMenu.Instance.SelectedTile = this;

        if(!discovered)
        {
            ContextMenu.Instance.ShowDiscoveryInfo();
            return;
        }

        ContextMenu.Instance.ShowTileInfo();
    }
}
