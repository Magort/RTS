using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Combat/Dice", order = 2)]

public class Dice : ScriptableObject
{
	public List<CombatAction> actions = new()
	{
		null, null, null, null, null, null
	};

	public List<CombatAction> CloneActions()
	{
		return new List<CombatAction>
		{ actions[0].CloneAction(), actions[1].CloneAction(), actions[2].CloneAction()
		, actions[3].CloneAction(), actions[4].CloneAction(), actions[5].CloneAction() };
	}
}
