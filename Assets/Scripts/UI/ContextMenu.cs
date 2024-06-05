using UnityEngine;

public class ContextMenu : MonoBehaviour
{
	public static ContextMenu Instance;
	public Tile SelectedTile;

	public ScoutingHandler scoutingPanel;
	public TileInfoPanel tileInfoPanel;
	public BuildingHandler buildingPanel;
	public UnitRecruitmentHandler unitRecruitmentPanel;

	float refreshTime = 1;
	float timer = 0;

	private void Awake()
	{
		Instance = this;
	}

	private void Update()
	{
		if (SelectedTile == null)
			return;

		if (SelectedTile.data.beingScouted)
			return;

		timer += Time.deltaTime;

		if (timer > refreshTime)
		{
			UpdatePanel();
			timer = 0;
		}
	}

	public void UpdatePanel()
	{
		if (SelectedTile == null)
			return;

		ShowTileInfo(SelectedTile.data.discovered, SelectedTile.data.affiliation);
	}

	public void ShowTileInfo(bool discovered, Affiliation affiliation)
	{
		tileInfoPanel.PopulatePanel(discovered);

		scoutingPanel.PopulateScoutingButton(!discovered);

		if (affiliation == Affiliation.Player && SelectedTile.unitRecruiters.Count > 0)
			unitRecruitmentPanel.PopulateUnitSlots();
		else
			unitRecruitmentPanel.ClearSlots();

		if (affiliation != Affiliation.Enemy && SelectedTile.data.units.Find(unit => unit.affiliation != Affiliation.Player) == null)
		{
			buildingPanel.PopulateBuildingsList(discovered);
		}
		else
			buildingPanel.ClearBuildingsList();
	}

	public void CloseAll()
	{
		scoutingPanel.gameObject.SetActive(false);
		tileInfoPanel.gameObject.SetActive(false);
		buildingPanel.panel.SetActive(false);
		unitRecruitmentPanel.gameObject.SetActive(false);

		SelectedTile = null;
	}
}
