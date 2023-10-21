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
}
