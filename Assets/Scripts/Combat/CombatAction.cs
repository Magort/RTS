using System.Collections.Generic;

[System.Serializable]
public class CombatAction
{
    public List<SubAction> subActions;
    public string name;

	[System.Serializable]
	public class SubAction
    {
		public int value;
		public Target target;
		public Effect effect;
        public Quantity quantity;
	}

    public CombatAction CloneAction()
    {
        return new CombatAction() { subActions = CloneSubActions(), name = name };
    }

    List<SubAction> CloneSubActions()
    {
        var clone = new List<SubAction>();

        foreach(SubAction subAction in subActions)
        {
            var newAction = new SubAction()
            {
                effect = subAction.effect,
                quantity = subAction.quantity,
                target = subAction.target,
                value = subAction.value
            };
            clone.Add(newAction);
        }

		return clone;
    }

    public enum Target
    {
        Ally,
        Opponent
    }

    public enum Quantity
    {
        Single,
        AoE
    }

    public enum Effect
    {
        Heal,
        Damage,
        Poison,
        Burn,
        Freeze,
        Haste,
        Disarm,
        Block
    }
}
