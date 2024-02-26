using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public static State state;
    public GameObject tileHover;

    public enum State
    {
        Idle,
        PlacingTile,
        BrowsingTile
    }

	private void Update()
	{
		if(state != State.PlacingTile)
        {
            tileHover.transform.position = SnapToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
	}

    Vector3 SnapToGrid(Vector3 position)
    {
        Vector3 snappedPosition = new();

        var tiles = Physics.OverlapSphere(position, 1.73f, LayerMask.GetMask("Tile"));
        if (tiles.Count() == 0)
            return position;

        var closestTile = ClosestTile(tiles.ToList(), position);


        return snappedPosition;
    }

    Tile ClosestTile(List<Collider> tiles, Vector3 position)
    {
        var currentDistance = 1.73f;
        Collider closest = null;
        
        foreach(Collider c in tiles)
        {
            if(Vector3.Distance(position, c.transform.position) <= currentDistance)
            {
                closest = c;
                currentDistance = Vector3.Distance(position, c.transform.position);
			}
        }

        return closest.GetComponent<Tile>();
    }

	public void SaveLevel()
    {
        var levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.tiles = CreateList();
#if UNITY_EDITOR
        AssetDatabase.CreateAsset(levelData, "Assets/LevelData/NewLevelData.asset");
        AssetDatabase.SaveAssets(); 
#endif
    }

    public void ClickSwitchState(int newState)
    {
        SwitchState((State)newState);
        tileHover.SetActive(true);
    }

    public static void SwitchState(State newState)
    {
        state = newState;
    }

    List<TileData> CreateList()
    {
        var list = new List<TileData>();

        foreach(Tile tile in TileGrid.Tiles)
        {
            list.Add(tile.data);
        }

        return list;
    }
}
