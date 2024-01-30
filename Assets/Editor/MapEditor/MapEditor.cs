using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public void SaveLevel()
    {
        var levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.tiles = CreateList();

		AssetDatabase.CreateAsset(levelData, "Assets/LevelData/NewLevelData.asset");
        AssetDatabase.SaveAssets();
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
