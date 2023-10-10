using UnityEngine;
using UnityEngine.UI;

public class UnitSlot : MonoBehaviour
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
}
