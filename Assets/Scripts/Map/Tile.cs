using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Diagnostics.Contracts;

public class Tile : MonoBehaviour, IPointerUpHandler
{
    public TileData data;
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
    [Header("Occupation")]
    public bool contested;
    public float contestPoints;
    public float maxContestPoints;
    private ProgressBar contestProgressBar;

    //Navigation
    public int g, h, F;

	private void Start()
	{
        data.spaceCoordinates = transform.position;

        foreach (var area in areas)
        {
            data.areas.Add(area.data);
        }

        if (data.neighbour)
            BecomeNeighbour();

        if (data.discovered)
            Reveal();
	}

	private void Update()
    {
        if (!data.discovered)
            return;

        if (contested)
        {
            contestPoints += Time.deltaTime;

            if (contestPoints > maxContestPoints)
            {
                CaptureTile();
            }
        }
    }

    public void InitializeTile(Vector3Int coordinates)
    {
        data.navigationCoordinates = coordinates;
		RollAreas();
    }

    public void LoadFromData(TileData loadData)
    {
        data.affiliation = loadData.affiliation;
        data.discovered = loadData.discovered;
        data.neighbour = loadData.neighbour;

       for(int i = 0; i < data.areas.Count; i++)
        {
            areas[i].data.type = loadData.areas[i].type;
        }
    }

    public void TransformIntoTile(Tile tile)
    {
        for(int i = 0; i < areas.Count; i++)
        {
            areas[i].data.type = tile.areas[i].data.type;
			areas[i].data.resourceAmount = tile.areas[i].data.resourceAmount;
		}
		data.affiliation = tile.data.affiliation;
    }

    public void RollAreas()
    {
        List<TileArea.Type> rolledAreas = new() { TileArea.Type.Empty };

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
            areas[i].data.type = rolledAreas[i];
            areas[i].data.resourceAmount = TileArea.ResourceStartingAmount[rolledAreas[i]];
        }
	}
	public void ShowDecorations()
    {
        foreach(TileArea area in areas)
        {
            area.ShowDecorations();
        }
    }
	public void ClearDecorations()
	{
		foreach (TileArea area in areas)
        {
            area.HideDecorations();
        }
	}

    public void MapGenerationAddUnit(MapUnit unit)
    {
		data.units.Add(unit);

		unit.currentTile = this;
	}

    public void AddUnit(MapUnit unit)
    {
		data.units.Add(unit);
        unit.currentTile = this;

        if(data.units.Count == 1)
        {
            unitSpot.ShowUnitModel(unit.affiliation);
        }

        ContextMenu.Instance.UpdatePanel();

        if (CombatHandler.CheckForCombat(this))
            return;

        if (unit.affiliation == Affiliation.Neutral && data.affiliation == Affiliation.Enemy)
            return;

        CheckForOccupation();
    }

    void CheckForOccupation()
    {
        if (data.affiliation == Affiliation.Neutral || data.units.Count == 0)
            return;

        foreach(var mapUnit in data.units)
        {
            if (mapUnit.affiliation == data.affiliation)
            {
                if(contested)
                {
                    contested = false;
                    contestPoints = 0;
                    contestProgressBar.ClearBar();
                    contestProgressBar = null;
                }

				return;
            }
        }

		contested = true;
        contestProgressBar = ProgressBarManager.Instance.GetProgressBar();
        contestProgressBar.ShowProgress(transform, maxContestPoints, "Contesting...");
    }

    public void CaptureTile()
    {
		contested = false;
        contestPoints = 0;
        KindgomLine.Instance.ChangeKingdomLine(this, false);
		data.affiliation = Affiliation.Neutral;
		areas.Where(area => area.data.type == TileArea.Type.Building).ToList().ForEach(area => area.RemoveBuilding());
        CheckGameObjectives();
	}

    void CheckGameObjectives()
    {
        if (data.navigationCoordinates == TileGrid.MainTile.data.navigationCoordinates)
        {
            GameEndHandler.Instance.LoseGame();
        }

        if (TileGrid.WinTargetTiles.Contains(this))
            GameEndHandler.Instance.ProgressWinCon(this);
    }

    public void RemoveUnit(MapUnit unit)
    {
		data.units.Remove(unit);

		if (data.units.Count == 0)
		{
			unitSpot.HideUnitModel();
		}
        else
        {
            unitSpot.ShowUnitModel(unit.affiliation);
        }

		ContextMenu.Instance.tileInfoPanel.PopulatePanel(true);

        //CheckForOccupation();
	}

    public void ChangeAffiliation(Affiliation affiliation)
    {
        if(affiliation != data.affiliation)
        {
			data.affiliation = affiliation;

            if(affiliation == Affiliation.Neutral)
            {
				KindgomLine.Instance.ChangeKingdomLine(this, false);
				return;
			}

            KindgomLine.Instance.ChangeKingdomLine(this, true);
            foreach(var area in areas)
            {
                area.lineRenderer.startColor = TileArea.affiliationToColor[affiliation];
                area.lineRenderer.endColor = TileArea.affiliationToColor[affiliation];
			}
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

		while (lighting.intensity < maxIntensity/5)
		{
			lighting.intensity += intensityGrowth;
			yield return waiter;
		}

		data.neighbour = true;
	}

	public void Reveal()
    {
		StartCoroutine(RevealCoroutine());
	}

    IEnumerator RevealCoroutine()
    {
        container.SetActive(true);

        float intensityGrowth = maxIntensity/100;
        WaitForSeconds waiter = new(0.01f);

		while(lighting.intensity < maxIntensity)
        {
            lighting.intensity += intensityGrowth;
            yield return waiter;
        }

		data.discovered = true;
		data.beingScouted = false;

		foreach (Tile tile in TileGrid.GetNeighbouringTiles(this))
		{
            if(!tile.data.neighbour)
			    tile.BecomeNeighbour();
		}

        ShowDecorations();
        if(data.units.Count > 0)
        {
            unitSpot.ShowUnitModel(data.units[0].affiliation);
        }    
	}

	public void OnDestroy()
	{
		TileGrid.Tiles.Remove(this);
	}

	public void OnPointerUp(PointerEventData eventData)
    {
		if (!data.neighbour)
		{
			ContextMenu.Instance.CloseAll();
			return;
		}

        if (GameState.TapLocked)
            return;

		if (UnitMovementHandler.Instance.selectedUnits.Count > 0)
		{
			if (!data.discovered)
				return;

			UnitMovementHandler.Instance.TryMove(this);
			return;
		}

        if (data.beingScouted)
		{
			ContextMenu.Instance.CloseAll();
			return;
		}

		AudioManager.Instance.Play(Sound.Name.Click);
		ContextMenu.Instance.SelectedTile = this;
		ContextMenu.Instance.ShowTileInfo(data.discovered, data.affiliation);
		UnitMovementHandler.Instance.DeselectAll();
	}
}
