using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public List<Building.Requirements.ResourceRequirement> startingResources;
   
    void Start()
    {
        GrantStartingResources();
        GameState.AddScouts(2);
        AudioManager.Instance.Play(Sound.Name.MainMusic);
        Application.targetFrameRate = 60;
        GameEventsManager.BuildingCompleted.AddListener(GameState.AddBuilding);
    }

    void GrantStartingResources()
    {
        foreach(var resource in startingResources)
        {
            GameState.AddResource(resource.resource, resource.amount);
        }
    }

    public static void SwitchPauseState(bool state)
    {
        if(state)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }    
    }    

}
