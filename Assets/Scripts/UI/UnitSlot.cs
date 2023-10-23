using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public UnitRecruiter unitRecruiter;

    public Image image;

    public void PopulateSlot(UnitRecruiter recruiter)
    {
        gameObject.SetActive(true);
        unitRecruiter = recruiter;
        image.sprite = unitRecruiter.unit.icon;
    }

    public void OnClick()
    {
        if (!unitRecruiter.CanAfford())
            return;

        unitRecruiter.Recruit();
		ContextMenu.Instance.tileInfoPanel.PopulatePresentMapUnits();
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverPanel.instance.PopulateHoverPanel(gameObject, unitRecruiter.unit.unitName, unitRecruiter.unit.Description());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverPanel.instance.DepopulateHoverPanel();
    }
}
