using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Combat/Dice", order = 2)]

public class Dice : ScriptableObject
{
    public List<CombatAction> actions = new()
    {
        null, null, null, null, null, null
    };
}
