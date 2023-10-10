using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUnitSlot : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    readonly Dictionary<Affiliation, Color> affiliationToColor = new()
    {
        { Affiliation.Player, Color.blue },
		{ Affiliation.Enemy, Color.red },
		{ Affiliation.Neutral, Color.green }
	};

    public void PopulateSlot(MapUnit mapUnit)
    {
        text.text = mapUnit.customName + " " + mapUnit.units.Count + "/" + MapUnit.armylimit;
        image.color = affiliationToColor[mapUnit.affiliation];
        gameObject.SetActive(true);
    }

    public void DepopulateSlot()
    {
        text.text = "";
        image.color = Color.white;
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        UnitMovementHandler.Instance.SelectUnit(ContextMenu.Instance.SelectedTile.units[transform.GetSiblingIndex()]);
    }
}
