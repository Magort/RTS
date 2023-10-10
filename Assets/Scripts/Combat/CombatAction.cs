using System.Collections.Generic;

[System.Serializable]
public class CombatAction
{
    public List<SubAction> subActions;

	[System.Serializable]
	public struct SubAction
    {
		public int value;
		public Target target;
		public Effect effect;
	}

    public enum Target
    {
        Ally,
        Enemy
    }

    public enum Effect
    {
        Heal,
        HealAll,
        Damage,
        DamageAll,
        DamageNextRound,
        Stun
    }
}
