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
        if(SelectedTile == null)
            return;

        if (SelectedTile.beingScouted)
            return;

        timer += Time.deltaTime;

        if (timer > refreshTime)
        {
            ShowTileInfo(SelectedTile.discovered, SelectedTile.affiliation);
            timer = 0;
        }
    }

    public void ShowTileInfo(bool discovered, Affiliation affiliation)
    {
        tileInfoPanel.PopulatePanel(discovered);

        scoutingPanel.gameObject.SetActive(!discovered);

        if(affiliation == Affiliation.Player && SelectedTile.unitRecruiters.Count > 0)
            unitRecruitmentPanel.PopulateUnitSlots();
        else
			unitRecruitmentPanel.ClearSlots();

		if (affiliation != Affiliation.Enemy)
        {
			buildingPanel.PopulateBuildingsList(discovered);
		}
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
