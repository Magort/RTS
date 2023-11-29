using UnityEngine;

public class DebugStick : MonoBehaviour
{
    public MapUnit unit;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(TileGrid.MainTile.coordinates);
		}
    }
}
