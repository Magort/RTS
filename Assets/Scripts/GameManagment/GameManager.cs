using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public int productionInterval = 5;
	public List<Building.Requirements.ResourceRequirement> startingResources;
   
    void Start()
    {
        GrantStartingResources();
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