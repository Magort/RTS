using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool unlocked;
    public Building building;

    public void OnClick()
    {
        if(BuildingHandler.Instance.TryBuild(building))
        {
            BuildingHandler.Instance.PopulateBuildingsList(true);
		}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverPanel.instance.PopulateHoverPanel(gameObject, building._name, building.Description() + "\n" + building.RequirementsToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverPanel.instance.DepopulateHoverPanel();
    }
}
