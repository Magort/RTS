using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    public static ContextMenu Instance;
    public Tile SelectedTile;

    public ScoutingHandler scoutingPanel;
    public TileInfoPanel tileInfoPanel;
    public BuildingHandler buildingPanel;
    public UnitRecruitmentHandler unitRecruitmentPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTileInfo(bool discovered, Affiliation affiliation)
    {
        tileInfoPanel.PopulatePanel(discovered);

        scoutingPanel.gameObject.SetActive(!discovered);

        if(affiliation == Affiliation.Player)
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
	}
}
