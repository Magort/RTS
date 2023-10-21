using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStick : MonoBehaviour
{
    public MapUnit unit;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ContextMenu.Instance.SelectedTile.AddUnit(unit);
        }
    }
}
