using System.Collections.Generic;
using UnityEngine;

public class Unit : ScriptableObject
{
    public List<Building.Requirements.ResourceRequirement> cost;
    [Header("Stats")]
    public int health;
    public int speed;
    public List<Dice> actions;

    public Dice RollAction()
    {
        return actions[Random.Range(0, 6)];
    }
}
