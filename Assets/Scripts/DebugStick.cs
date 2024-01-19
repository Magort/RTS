using TMPro;
using UnityEngine;

public class DebugStick : MonoBehaviour
{
    public MapUnit unit;
    public TextMeshProUGUI fpsCounter;
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log(TileGrid.MainTile.coordinates);
        //}

        UpdateFPS();
    }

    public void UpdateFPS()
    {
        fpsCounter.text = Mathf.RoundToInt(1 / Time.unscaledDeltaTime).ToString();
    }
}
