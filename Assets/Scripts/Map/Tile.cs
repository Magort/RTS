using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerUpHandler
{
    [Header("Data")]
    public bool discovered;
    public bool neighbour;
    public List<MapUnit> units;
    public Affiliation affiliation;
    public Vector3Int coordinates;
	[Header("Light")]
    public Light lighting;
    public float maxIntensity;
    [Header("Tile Areas")]
    public GameObject container;
    public Animator containerAnimator;
	public List<TileArea> areas;
    public Dictionary<TileArea.Type, GameObject> typeToDecoration = new();
    public TileUnitSpot unitSpot;
    public List<UnitRecruiter> unitRecruiters;

    //Navigation
    public int g, h, F;

    public void InitializeTile(Vector3Int coordinates)
    {
        this.coordinates = coordinates;
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

            if (TileGrid.GetAreaTypeCount(rolledAreas, type) < TileArea.MaxSameType[type])
                rolledAreas.Add(type);
            else
                rolledAreas.Add(TileArea.Type.Empty);
        }

        for(int i = 0; i < rolledAreas.Count; i++)
        {
            areas[i].type = rolledAreas[i];
            areas[i].resourceAmount = TileArea.ResourceStartingAmount[rolledAreas[i]];
        }
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
		foreach (TileArea area in areas)
        {
            area.HideDecorations();
        }
	}

    public void AddUnit(MapUnit unit)
    {
        units.Add(unit);
        unit.currentTile = this;

        if(units.Count == 1)
        {
            unitSpot.ShowUnitModel(unit.affiliation);
        }

        ContextMenu.Instance.tileInfoPanel.PopulatePanel(true);
    }

    public void RemoveUnit(MapUnit unit)
    {
        units.Remove(unit);

		if (units.Count == 0)
		{
			unitSpot.HideUnitModel();
		}
        else
        {
            unitSpot.ShowUnitModel(unit.affiliation);
        }

		ContextMenu.Instance.tileInfoPanel.PopulatePanel(true);
	}

    public void ChangeAffiliation(Affiliation affiliation)
    {
        if(affiliation != this.affiliation)
        {
            this.affiliation = affiliation;
            if(affiliation == Affiliation.Player)
                KindgomLine.Instance.AddTileToBorder(this);
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

        container.SetActive(true);
        containerAnimator.Play("Show");

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

		foreach (Tile tile in TileGrid.GetNeighbouringTiles(this))
		{
            if(!tile.neighbour)
			    tile.BecomeNeighbour();
		}

        ShowDecorations();
	}

    public void OnPointerUp(PointerEventData eventData)
    {
		if (!neighbour)
		{
			ContextMenu.Instance.CloseAll();
			return;
		}

		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (!discovered)
				return;

			UnitMovementHandler.Instance.TryMove(this);
			return;
		}

		ContextMenu.Instance.SelectedTile = this;
		ContextMenu.Instance.ShowTileInfo(discovered, affiliation);
		UnitMovementHandler.Instance.Deselect();
	}
}
