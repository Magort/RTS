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
    public Sprite icon;

    public CombatAction RollAction()
    {
        return dice.actions[Random.Range(0, 6)];
    }
}
