using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapUnitWindowDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public TextMeshProUGUI textBox;
	public MapUnitWindowDropArea.AreaType areaType;
	public void OnBeginDrag(PointerEventData eventData)
	{
		DragBox.Instance.EnableDragBox(textBox.text);
		MapUnitWindow.Instance.ToggleDropAreas(true);
	}

	public void OnDrag(PointerEventData eventData)
	{

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DisableDraggingTools();
	}

	public void DisableDraggingTools()
	{
		DragBox.Instance.DisableDragBox();
		MapUnitWindow.Instance.ToggleDropAreas(false);
	}

	private void OnDisable()
	{
		DisableDraggingTools();
	}

}
