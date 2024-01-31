using System.Collections.Generic;
using UnityEngine;

public class CampaingButton : MonoBehaviour
{
    public CampaignLevelsContainer container;
    public List<LevelData> levels;

    public void ShowLevelsList()
    {
        container.PopulateLevelsList(levels);
    }
}
