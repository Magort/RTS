using UnityEngine;

public class BuildingSlot : MonoBehaviour
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
}
