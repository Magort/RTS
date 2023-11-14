using System.Collections.Generic;

public static class IconIDs
{
    public static Dictionary<CombatAction.Effect, int> effectToIconID = new()
    {
        { CombatAction.Effect.Heal, 2 },
		{ CombatAction.Effect.Damage, 3 },
		{ CombatAction.Effect.Block, 4 },
		{ CombatAction.Effect.Poison, 5 },
		{ CombatAction.Effect.Burn, 10 },
		{ CombatAction.Effect.Disarm, 11 },
		{ CombatAction.Effect.Haste, 12 },
		{ CombatAction.Effect.Freeze, 13 },
		{ CombatAction.Effect.Wound, 14 },
		{ CombatAction.Effect.Shatter, 15 }
	};

	public static Dictionary<(CombatAction.Quantity, CombatAction.Target), int> quantityToIconID = new()
	{
		{ (CombatAction.Quantity.Single, CombatAction.Target.Ally), 6 },
		{ (CombatAction.Quantity.AoE, CombatAction.Target.Ally), 7 },
		{ (CombatAction.Quantity.Single, CombatAction.Target.Opponent), 16 },
		{ (CombatAction.Quantity.AoE, CombatAction.Target.Opponent), 17 }

	};

	public static Dictionary<Resource, int> resourceToIconID = new()
	{
		{ Resource.Wood, 0 },
		{ Resource.Food, 1 },
		{ Resource.Gold, 8 },
		{ Resource.Essence, 9 }
	};
}
