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
        StartCoroutine(ResourceProduction());
    }

    void GrantStartingResources()
    {
        foreach(var resource in startingResources)
        {
            GameState.AddResource(resource.resource, resource.amount);
        }
    }

    IEnumerator ResourceProduction()
    {
        var waiter = new WaitForSeconds(productionInterval);

        while(true)
        {
            yield return waiter;

            foreach(var resource in GameState.ResourcesGrowth)
            {
				GameState.AddResource(resource.Key, resource.Value);
			}
        }
    }

}
