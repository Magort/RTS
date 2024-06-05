using UnityEngine;
using UnityEngine.EventSystems;

public class MapUnitWindowDropArea : MonoBehaviour, IDropHandler
{
	public AreaType areaType;
	public enum AreaType
	{
		PlayerArmy,
		EnemyArmy,
		MapUnitArea1,
		MapUnitArea2,
		Trash
	}

	public void OnDrop(PointerEventData eventData)
	{
		var draggedSlot = eventData.pointerDrag.GetComponent<MapUnitWindowDragHandler>();

		switch (areaType)
		{
			case AreaType.MapUnitArea1:
				if (draggedSlot.areaType == AreaType.PlayerArmy || draggedSlot.areaType == AreaType.EnemyArmy)
				{
					MapUnitWindow.Instance.SelectMapUnit(areaType,
						draggedSlot.GetComponent<MapUnitWindowSlot>().mapUnit);
				}
				if (draggedSlot.areaType == AreaType.MapUnitArea2)
				{
					MapUnitWindow.Instance.MoveUnit(areaType,
						draggedSlot.GetComponent<MapUnitWindowSlot>().unit);
				}
				break;

			case AreaType.MapUnitArea2:
				if (draggedSlot.areaType == AreaType.PlayerArmy)
				{
					MapUnitWindow.Instance.SelectMapUnit(areaType,
						draggedSlot.GetComponent<MapUnitWindowSlot>().mapUnit);
				}
				if (draggedSlot.areaType == AreaType.MapUnitArea1)
				{
					MapUnitWindow.Instance.MoveUnit(areaType,
						draggedSlot.GetComponent<MapUnitWindowSlot>().unit);
				}
				break;

			default:
				break;
		}
	}
}
