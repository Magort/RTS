using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Tile : MonoBehaviour, IPointerUpHandler
{
    [Header("Data")]
    public bool discovered;
    public bool neighbour;
    public bool beingScouted;
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
    [Header("Occupation")]
    public bool contested;
    public float contestPoints;
    public float maxContestPoints;
    private ProgressBar contestProgressBar;

    //Navigation
    public int g, h, F;

    private void Update()
    {
        if (!discovered)
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
        affiliation = tile.affiliation;

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

    public void MapGenerationAddUnit(MapUnit unit)
    {
		units.Add(unit);

		unit.currentTile = this;
	}

    public void AddUnit(MapUnit unit)
    {
        units.Add(unit);
        unit.currentTile = this;

        if(units.Count == 1)
        {
            unitSpot.ShowUnitModel(unit.affiliation);
        }

        ContextMenu.Instance.UpdatePanel();

        if (CombatHandler.CheckForCombat(this))
            return;

        if (unit.affiliation == Affiliation.Neutral && affiliation == Affiliation.Enemy)
            return;

        CheckForOccupation();
    }

    void CheckForOccupation()
    {
        if (affiliation == Affiliation.Neutral || units.Count == 0)
            return;

        foreach(var mapUnit in units)
        {
            if (mapUnit.affiliation == affiliation)
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
		affiliation = Affiliation.Neutral;
		areas.Where(area => area.type == TileArea.Type.Building).ToList().ForEach(area => area.RemoveBuilding());
        CheckGameObjectives();
	}

    void CheckGameObjectives()
    {
        if (coordinates == TileGrid.MainTile.coordinates)
        {
            GameEndHandler.Instance.LoseGame();
        }

        if (TileGrid.WinTargetTiles.Contains(this))
            GameEndHandler.Instance.ProgressWinCon(this);
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

        //CheckForOccupation();
	}

    public void ChangeAffiliation(Affiliation affiliation)
    {
        if(affiliation != this.affiliation)
        {
            this.affiliation = affiliation;

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
        beingScouted = false;

		foreach (Tile tile in TileGrid.GetNeighbouringTiles(this))
		{
            if(!tile.neighbour)
			    tile.BecomeNeighbour();
		}

        ShowDecorations();
        if(units.Count > 0)
        {
            unitSpot.ShowUnitModel(units[0].affiliation);
        }    
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
			if (!discovered || CameraController.Instance.dragged)
				return;

			UnitMovementHandler.Instance.TryMove(this);
			return;
		}

        if (beingScouted)
		{
			ContextMenu.Instance.CloseAll();
			return;
		}

		AudioManager.Instance.Play(Sound.Name.Click);
		ContextMenu.Instance.SelectedTile = this;
		ContextMenu.Instance.ShowTileInfo(discovered, affiliation);
		UnitMovementHandler.Instance.Deselect();
	}
}
