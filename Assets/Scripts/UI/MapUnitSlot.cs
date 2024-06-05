using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUnitSlot : MonoBehaviour
{
	public TextMeshProUGUI text;
	public Image affiliationImage;
	public Image backgroundImage;
	public Button button;
	readonly Dictionary<Affiliation, Color> affiliationToColor = new()
	{
		{ Affiliation.Player, Color.blue },
		{ Affiliation.Enemy, Color.red },
		{ Affiliation.Neutral, Color.green }
	};

	public Color notSelected;
	public Color selected;

	private void Start()
	{
		backgroundImage.color = notSelected;
	}

	public void PopulateSlot(MapUnit mapUnit)
	{
		text.text = mapUnit.customName + " " + mapUnit.units.Count;
		affiliationImage.color = affiliationToColor[mapUnit.affiliation];
		SetBackgroundColor();

		if (mapUnit.affiliation != Affiliation.Player)
		{
			button.interactable = false;
		}
		else
		{
			text.text += "/" + MapUnit.playerArmylimit;
		}
		gameObject.SetActive(true);
	}

	public void DepopulateSlot()
	{
		text.text = "";
		affiliationImage.color = Color.white;
		button.interactable = true;
		gameObject.SetActive(false);
	}

	public void OnClick()
	{
		AudioManager.Instance.Play(Sound.Name.Click);

		var unit = ContextMenu.Instance.SelectedTile.data.units[transform.GetSiblingIndex()];

		if (!UnitMovementHandler.Instance.selectedUnits.Contains(unit))
		{
			UnitMovementHandler.Instance.SelectUnit(unit);
		}
		else
		{
			UnitMovementHandler.Instance.Deselect(unit);
		}

		SetBackgroundColor(unit);
	}

	void SetBackgroundColor()
	{
		if (UnitMovementHandler.Instance.selectedUnits.Contains(ContextMenu.Instance.SelectedTile.data.units[transform.GetSiblingIndex()]))
		{
			backgroundImage.color = selected;
		}
		else
		{
			backgroundImage.color = notSelected;
		}
	}
	void SetBackgroundColor(MapUnit unit)
	{
		if (UnitMovementHandler.Instance.selectedUnits.Contains(unit))
		{
			backgroundImage.color = selected;
		}
		else
		{
			backgroundImage.color = notSelected;
		}
	}
}
