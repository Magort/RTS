using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Combat/Unit", order = 1)]
public class Unit : ScriptableObject
{
    public List<Building.Requirements.ResourceRequirement> recruitCost;
	public List<Building.Requirements.ResourceRequirement> upkeepCost;

	[Header("Stats")]
    public int health;
    public int speed;
    public Dice dice;

    [Header("Identification")]
    public string unitName;
    public Sprite icon;

    public (CombatAction, CombatAction) RollActions()
    {
        var tempActions = dice.CloneActions();

        var action1 = tempActions[Random.Range(0, 6)];

        tempActions.Remove(action1);

        var action2 = tempActions[Random.Range(0, 5)];

        return (action1, action2);
    }

    public string Description()
    {
        string description = "<b>Health</b>: " + health + "\t<b>Speed</b>: " + speed + "\n<b>Actions</b>: | ";

        foreach(var action in dice.actions)
        {
            foreach(var subAction in action.subActions)
            {
                description += subAction.value
                    + "<sprite=" + IconIDs.effectToIconID[subAction.effect] + ">"
					+ "<sprite=" + IconIDs.quantityToIconID[(subAction.quantity, subAction.target)] + ">";
            }
            description += " | ";
        }

        description += "\n<b>Recruit Cost:</b>";

        foreach(var cost in recruitCost)
        {
            description += "<sprite=" + IconIDs.resourceToIconID[cost.resource] + ">" + cost.amount + " ";
        }

		description += "\n<b>Upkeep Cost:</b>";

		foreach (var cost in upkeepCost)
		{
			description += "<sprite=" + IconIDs.resourceToIconID[cost.resource] + ">" + cost.amount + " ";
		}

		return description;
    }
}
