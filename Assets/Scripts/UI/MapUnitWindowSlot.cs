using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapUnitWindowSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[HideInInspector]
	public MapUnit mapUnit = null;
	[HideInInspector]
	public Unit unit = null;

	public TextMeshProUGUI textBox;
	public Selectable selectable;

	public void PopulateSlot(MapUnit mapUnit)
	{
		this.mapUnit = mapUnit;
		textBox.text = mapUnit.customName;
		gameObject.SetActive(true);
	}
	public void PopulateSlot(Unit unit)
	{
		this.unit = unit;
		textBox.text = unit.unitName;
		gameObject.SetActive(true);
	}

	public void SwitchDraggable(bool state)
	{
		selectable.interactable = state;
	}

	public void ClearSlot()
	{
		mapUnit = null;
		unit = null;
		textBox.text = "";
		gameObject.SetActive(false);
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		if(unit != null)
			HoverPanel.instance.PopulateHoverPanel(gameObject, unit.unitName, unit.Description());
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HoverPanel.instance.DepopulateHoverPanel();
	}
}
