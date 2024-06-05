using UnityEngine;

[CreateAssetMenu(fileName = "MissionObjective", menuName = "MissionObjectives/TileSelect", order = 6)]
public class MOTileSelect : MissionObjective
{
	public Vector3Int tileToSelect;

	public override void Clear()
	{
		GameEventsManager.TileSelected.RemoveListener(CheckTile);
		TileGrid.Tiles.Find(tile => tile.data.navigationCoordinates == tileToSelect).SwitchParticles();
	}

	public override bool ConditionsMet()
	{
		return ContextMenu.Instance.SelectedTile.data.navigationCoordinates == tileToSelect;
	}

	public override void Innit()
	{
		GameEventsManager.TileSelected.AddListener(CheckTile);
		TileGrid.Tiles.Find(tile => tile.data.navigationCoordinates == tileToSelect).SwitchParticles();
	}

	void CheckTile(Vector3Int coords)
	{
		if (coords == tileToSelect)
			ProgressQuantity();
	}
}
