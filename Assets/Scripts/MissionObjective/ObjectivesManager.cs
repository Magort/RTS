using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;

    public List<MissionObjective> MissionObjectives;

    int currentQuantity = 0;
    int currentTimer = 0;
    int currentObjective = 0;

    readonly WaitForSeconds waiter = new(1);

	private void Start()
	{
		Instance = this;
	}

	public void ProgressQuantity()
    {
        currentQuantity++;
        if(currentQuantity >= MissionObjectives[currentObjective].quantityToPass)
            CompleteObjective();
    }

    public void CompleteObjective()
    {
        //Grant Rewards

        if(currentObjective == MissionObjectives.Count)
        {
            //end game
        }
        else
        {
			SetObjective(currentObjective + 1);
            //update display
		}
    }

    public void SetObjective(int index)
    {
        currentObjective = index;
        currentQuantity = 0;

        if (MissionObjectives[currentObjective].timeToPass > 0)
            StartCoroutine(ProgressOverTime());
    }

    IEnumerator ProgressOverTime()
    {
        currentTimer = 0;

        while(true)
        {
            if(currentTimer >= MissionObjectives[currentObjective].timeToPass)
            {
                CompleteObjective();
                break;
            }

            yield return waiter;

            if (MissionObjectives[currentObjective].ConditionsMet())
            {
                currentTimer++;
                //Update display
            }
        }
    }
}
