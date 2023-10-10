using System.Collections.Generic;
using UnityEngine;

public class UnitRecruitmentHandler : MonoBehaviour
{
    public List<UnitSlot> slots;

    public void PopulateUnitSlots()
    {
        ClearSlots();
        gameObject.SetActive(true);

        for(int i = 0; i < ContextMenu.Instance.SelectedTile.unitRecruiters.Count; i++)
        {
            slots[i].PopulateSlot(ContextMenu.Instance.SelectedTile.unitRecruiters[i]);
        }
    }

    public void ClearSlots()
    {
        foreach (var slot in slots)
        {
            slot.unitRecruiter = null;
            slot.gameObject.SetActive(false);
        }

		gameObject.SetActive(false);
	}    
}
