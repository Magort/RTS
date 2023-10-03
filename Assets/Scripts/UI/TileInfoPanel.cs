using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    [TextArea] public string notDiscoveredText;

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void PopulatePanel(bool discovered)
    {
        gameObject.SetActive(true);

        if(!discovered)
        {
            infoText.text = notDiscoveredText;
            return;
        }


        infoText.text = TileInformation();
    }

    string TileInformation()
    {
        string info = "";

        Dictionary<Resource, int> addedResources = new()
        {
            {Resource.Wood, 0 },
			{Resource.Food, 0 },
			{Resource.Gold, 0 },
			{Resource.Essence, 0 }
		};

        foreach(TileArea area in ContextMenu.Instance.SelectedTile.areas)
        {
            switch(area.type)
            {
                case TileArea.Type.EssenceSource:
                    addedResources[Resource.Essence] += area.resourceAmount;
                    break;
				case TileArea.Type.WoodSource:
                    addedResources[Resource.Wood] += area.resourceAmount;
					break;
				case TileArea.Type.FoodSource:
                    addedResources[Resource.Food] += area.resourceAmount;
					break;
				case TileArea.Type.GoldSource:
                    addedResources[Resource.Gold] += area.resourceAmount;
					break;
                default:
                    break;
			}
        }

        foreach(var addedResource in addedResources)
        {
            if(addedResource.Value > 0)
            {
                info += addedResource.Key + ": " + addedResource.Value + "\n";
            }
        }

        if(info.Length == 0)
        {
            info = "There are no Resources here, but worry not, sir! It's an execelent spot for our Buildings!";
        }

        return info;
    }
}
