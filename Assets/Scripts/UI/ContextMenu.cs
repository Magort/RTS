using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    public static ContextMenu Instance;
    public Tile SelectedTile;

    public ScoutingHandler scoutingPanel;
    public TileInfoPanel tileInfoPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTileInfo()
    {
        tileInfoPanel.gameObject.SetActive(true);
    }

    public void ShowDiscoveryInfo()
    {
        scoutingPanel.gameObject.SetActive(true);
    }

    public void CloseAll()
    {
		scoutingPanel.gameObject.SetActive(false);
		tileInfoPanel.gameObject.SetActive(false);
	}
}
