using System.Collections.Generic;

public static class CombatActionToIconID
{
    public static Dictionary<CombatAction.Effect, int> effectToIconID = new()
    {
        { CombatAction.Effect.Heal, 0 },
		{ CombatAction.Effect.Damage, 1 },
		{ CombatAction.Effect.Block, 2 },
		{ CombatAction.Effect.Poison, 3 },
		{ CombatAction.Effect.Burn, 4 },
		{ CombatAction.Effect.Disarm, 5 },
		{ CombatAction.Effect.Haste, 6 },
		{ CombatAction.Effect.Freeze, 7 }
	};

	public static Dictionary<CombatAction.Quantity, int> quantityToIconID = new()
	{
		{ CombatAction.Quantity.Single, 8 },
		{ CombatAction.Quantity.AoE, 9 }
	};
}
