using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Runtime.CompilerServices;

public class Tile : MonoBehaviour, IPointerUpHandler
{
    public TileData data;
    public GameObject particles;
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

	private void Awake()
	{
        if (areas.Count == 0)
            return;

		for (int i = 0; i < data.areas.Count; i++)
        {
            areas[i].data = data.areas[i];
        }
    }

	private void Start()
	{
        data.spaceCoordinates = transform.position;

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
        areas.ForEach(area => area.InitDictionary());
		RollAreas();
    }

    public void LoadFromData(TileData loadData)
    {
        data.discovered = loadData.discovered;
        data.neighbour = loadData.neighbour;
        data.navigationCoordinates = loadData.navigationCoordinates;

        foreach (var unit in loadData.units)
        {
            AddUnit(unit);

            if (unit.affiliation == Affiliation.Player)
            {
                foreach (var recrutableUnit in unit.units)
                {
                    foreach (var cost in recrutableUnit.upkeepCost)
                        UnitUpkeepHandler.Instance.AddUpkeep(cost.amount, cost.resource);
                }
            }
        }

        for(int i = 0; i < data.areas.Count; i++)
        {
            areas[i].data.type = loadData.areas[i].type;
            areas[i].data.resourceAmount = loadData.areas[i].resourceAmount;
            areas[i].InitDictionary();
            if (areas[i].data.type == TileArea.Type.Building)
            {
                areas[i].data.building = loadData.areas[i].building;
				BuildingHandler.Instance.LoadBuilding(this, areas[i], loadData.affiliation == Affiliation.Player);
			}
        }

        if (data.neighbour)
        {
            container.SetActive(true);
            lighting.intensity = maxIntensity/5;
        }

        if (data.discovered)
        {
			ShowDecorations();
            if (data.units.Count > 0)
            {
                unitSpot.ShowUnitModel(data.units[0].affiliation);
            }
		}

        ChangeAffiliation(loadData.affiliation);
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

    public void SwitchParticles()
    {
        particles.SetActive(!particles.activeSelf);
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

        if (unit.affiliation == Affiliation.Player)
            GameEventsManager.TileControlled.Invoke(data.navigationCoordinates);

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
        contestProgressBar.ShowProgress(transform, maxContestPoints, "Contesting...", TileArea.affiliationToColor[data.units[0].affiliation]);
    }

    public void CaptureTile()
    {
		contested = false;
        contestPoints = 0;
        KindgomLine.Instance.ChangeKingdomLine(this, false);
		data.affiliation = Affiliation.Neutral;
		areas.Where(area => area.data.type == TileArea.Type.Building).ToList().ForEach(area => area.RemoveBuilding());
	}

    public void RemoveUnit(MapUnit unit)
    {
		data.units.Remove(unit);

		if (data.units.Count == 0)
		{
			unitSpot.HideUnitModel();

            if (unit.affiliation == Affiliation.Player)
                GameEventsManager.TileLostControll.Invoke(data.navigationCoordinates);
		}
        else
        {
            unitSpot.ShowUnitModel(unit.affiliation);
        }

		ContextMenu.Instance.tileInfoPanel.PopulatePanel(true);
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

        GameEventsManager.TileDiscovered.Invoke(data.navigationCoordinates);
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
        GameEventsManager.TileSelected.Invoke(data.navigationCoordinates);
		UnitMovementHandler.Instance.DeselectAll();
	}
}
