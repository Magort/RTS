using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;

    public static List<MissionObjective> MissionObjectives;

	public static int currentQuantity = 0;
	public static int currentTimer = 0;
	public static int currentObjective = 0;

    readonly WaitForSeconds waiter = new(1);

	private void Awake()
	{
		Instance = this;
	}

    public void LoadObjectives(List<MissionObjective> objectives)
    {
        MissionObjectives = objectives;
        SetObjective(0);
    }

    public static MissionObjective GetCurrentObjective()
    {
        return MissionObjectives[currentObjective];
    }

	public void ProgressQuantity()
    {
        currentQuantity++;

        if(currentQuantity >= MissionObjectives[currentObjective].quantityToPass)
            CompleteObjective();

        MissionObjectivePanel.instance.UpdateDisplay();
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
		}
    }

    public void SetObjective(int index)
    {
        if(index == 0)
		    MissionObjectives[currentObjective].Clear();

		currentObjective = index;
        currentQuantity = 0;

        if (MissionObjectives[currentObjective].timeToPass > 0)
            StartCoroutine(ProgressOverTime());

        MissionObjectives[currentObjective].Innit();
		MissionObjectivePanel.instance.UpdateDisplay();
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
				MissionObjectivePanel.instance.UpdateDisplay();
			}
        }
    }
}
