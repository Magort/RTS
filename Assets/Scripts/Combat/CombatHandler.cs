using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CombatAction;

public static class CombatHandler
{
    public static Army playerArmy = new();
    public static Army opponentArmy = new();
    public static List<MapUnit> presentMapUnits = new();
    public static Tile tileFoughtOn = null;
    
    public static int actionsMade = 0;
    public static int maxPlayerActions = 0;

    public class Army
    {
        public int maxHealth;
        public int health;
        public int speed;
        public int block;
        public int wounds;

        public List<CombatStatus> statuses;
        public MapUnit mapUnit;

        public List<int> frozen;
		public List<int> disarmed;
		public List<int> hasted;

		public void InitializeArmy(MapUnit mapUnit)
        {
            this.mapUnit = mapUnit;
            maxHealth = mapUnit.GetHealthSum();
            health = mapUnit.GetHealthSum();
            speed = mapUnit.GetSpeedSum();
            block = 0;
            wounds = 0;
            statuses = new List<CombatStatus>();
            frozen = new List<int>();
            disarmed = new List<int>();
            hasted = new List<int>();
        }
        public void NewRoundReset()
        {
			frozen = new List<int>();
			disarmed = new List<int>();
			hasted = new List<int>();
            block = 0;
		}
        public void TakeDamage(int value)
        {
            health -= Mathf.Max(0, value - block + wounds);
        }
        public void HealHealth(int value)
        {
            health += value;
            ChangeWounds(-value);
        }
        public void AddCombatStatus(CombatStatus status, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                statuses.Add(status);
            }
        }
        public void RemoveCombatStatus(CombatStatus status, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
				statuses.Remove(status);
			}
        }
        public void ChangeBlock(int value)
        {
            block += value;
            block = Mathf.Clamp(block, 0, 5);
        }
        public void ChangeWounds(int value)
        {
			wounds = Mathf.Max(0, wounds + value);
		}
        public int GetStatusAmount(CombatStatus status)
        {
            int amount = 0;

            foreach(var cStatus in statuses)
            {
                if (cStatus == status)
                    amount++;
            }

            return amount;
        }
    }

    public static void CheckForCombat(Tile tile)
    {
        if (tile.units.Find(unit => unit.affiliation == Affiliation.Player) != null
            && tile.units.Find(unit => unit.affiliation != Affiliation.Player) != null)
        {
            StartCombat(tile);
        }
    }

    public static void CheckForPlayerTurnEnd()
    {
        actionsMade++;

        if (actionsMade >= maxPlayerActions)
            HandleEnemyTurn();
    }

    public static void StartCombat(Tile tile)
    {
        GameManager.SwitchPauseState(true);

        tileFoughtOn = tile;
        presentMapUnits = new();

        foreach (MapUnit unit in tile.units)
        {
            presentMapUnits.Add(unit);
        }    

        presentMapUnits.OrderByDescending(army => army.units.Count).ThenByDescending(army => army.GetSpeedSum());

        playerArmy = new();
        playerArmy.InitializeArmy(GetNextMapUnit(Affiliation.Player));

        opponentArmy = new();
        opponentArmy.InitializeArmy(GetNextMapUnit(Affiliation.Enemy));

		CombatPanel.Instance.PopulatePanel();
	}

    public static void StartNewRound()
    {
		playerArmy.NewRoundReset();
		opponentArmy.NewRoundReset();

        CombatPanel.Instance.UpdateStats();

        actionsMade = 0;
        maxPlayerActions = 0;

		HandlePlayerTurn();
	}

    public static void EndRound()
    {
        CombatPanel.Instance.SwitchRollButton(true);
        if(CheckForArmyDeath(playerArmy))
        {
			HandleArmyDeath(Affiliation.Player);
		}
	}

    static void HandlePlayerTurn()
    {
		if (CheckForArmyDeath(playerArmy))
		{
			HandleArmyDeath(Affiliation.Player);
			return;
		}

		ResolveStatuses(playerArmy);

		if (CheckForArmyDeath(playerArmy))
		{
			HandleArmyDeath(Affiliation.Player);
			return;
		}

		CombatPanel.Instance.ShowActionRolls(Affiliation.Player);
	}

    static void HandleEnemyTurn()
    {
        if(CheckForArmyDeath(opponentArmy))
        {
            HandleArmyDeath(Affiliation.Enemy);
            return;
        }

		ResolveStatuses(opponentArmy);

		if (CheckForArmyDeath(opponentArmy))
		{
			HandleArmyDeath(Affiliation.Enemy);
			return;
		}

		CombatPanel.Instance.ShowActionRolls(Affiliation.Enemy);
	}

    static bool CheckForArmyDeath(Army army)
    {
        if(army.health <= 0)
        {
            presentMapUnits.Remove(army.mapUnit);
			return true;
        }

        return false;
    }

    static void HandleArmyDeath(Affiliation affiliation)
    {
        var newUnit = GetNextMapUnit(affiliation);

        if (newUnit == null)
        {
            EndCombat();
            return;
        }

        if(affiliation == Affiliation.Player)
        {
            playerArmy.InitializeArmy(newUnit);
            HandlePlayerTurn();
        }
        else
        {
			opponentArmy.InitializeArmy(newUnit);
            HandleEnemyTurn();
		}
	}

    static void EndCombat()
    {
        CombatPanel.Instance.HidePanel();

        tileFoughtOn.units = new();

        foreach(var unit in presentMapUnits)
        {
			tileFoughtOn.AddUnit(unit);
		}
        
		GameManager.SwitchPauseState(false);
	}

	static MapUnit GetNextMapUnit(Affiliation affiliation)
    {
		MapUnit unit = null;

		if (affiliation == Affiliation.Player)
        {
			if (presentMapUnits.Find(army => army.affiliation == Affiliation.Player) != null)
            {
                unit = presentMapUnits.First(army => army.affiliation == Affiliation.Player);
            }
            return unit;
		}
        else
        {
            if (presentMapUnits.Find(army => army.affiliation != Affiliation.Player) != null)
            {
                unit = presentMapUnits.First(army => army.affiliation != Affiliation.Player);
            }
			return unit;
		}
    }

    public static void ResolveStatuses(Army army)
    {
		List<int> availableUnits = UnitIDList(army.mapUnit);

        int freezeCount = army.GetStatusAmount(CombatStatus.Freeze);
		int disarmCount = army.GetStatusAmount(CombatStatus.Disarm);

		for (int i = 0; i < freezeCount; i++)
		{
            var roll = Random.Range(0, availableUnits.Count);
            army.frozen.Add(roll);
			army.RemoveCombatStatus(CombatStatus.Freeze, 1);
			availableUnits.Remove(roll);
		}
		for (int i = 0; i < disarmCount; i++)
		{
			var roll = Random.Range(0, availableUnits.Count);
			army.disarmed.Add(roll);
			army.RemoveCombatStatus(CombatStatus.Disarm, 1);
			availableUnits.Remove(roll);
		}
		for (int i = 0; i < army.GetStatusAmount(CombatStatus.Haste); i++)
		{
			var roll = Random.Range(0, availableUnits.Count);
			army.hasted.Add(roll);
			army.RemoveCombatStatus(CombatStatus.Haste, 1);
		}

        army.TakeDamage(army.GetStatusAmount(CombatStatus.Poison));
        army.TakeDamage(army.GetStatusAmount(CombatStatus.Burn) * army.mapUnit.units.Count);

        army.RemoveCombatStatus(CombatStatus.Burn, army.GetStatusAmount(CombatStatus.Burn) / 2);

		CombatPanel.Instance.UpdateStats();
	}

    public static List<(CombatAction, CombatAction)> RollActions(Army army)
    {
        List<(CombatAction, CombatAction)> actions = new();
        
        for(int i = 0; i < army.mapUnit.units.Count; i++)
        {
			if (army.frozen.Contains(i))
            {
                continue;
            }

            var rolledActions = army.mapUnit.units[i].RollActions();

            if (army.disarmed.Contains(i))
            {
                actions.Add(DisarmActions(rolledActions));
            }
            else
                actions.Add(rolledActions);


			if (army.hasted.Contains(i))
			{
				var rolledHastedActions = army.mapUnit.units[i].RollActions();

				if (army.disarmed.Contains(i))
					actions.Add(DisarmActions(rolledHastedActions));
				else
					actions.Add(rolledHastedActions);
			}
		}

        maxPlayerActions = actions.Count;

        return actions;
    }

    static (CombatAction, CombatAction) DisarmActions((CombatAction, CombatAction) actions)
    {
		var subActions1 = actions.Item1.subActions;
		var subActions2 = actions.Item2.subActions;

		subActions1.Where(subAction => subAction.effect == Effect.Damage).ToList()
					.ForEach(subAction => subAction.value = 0);

		subActions2.Where(subAction => subAction.effect == Effect.Damage).ToList()
					.ForEach(subAction => subAction.value = 0);

		actions.Item1.subActions = subActions1;
		actions.Item2.subActions = subActions2;

        return actions;
	}

    static List<int> UnitIDList(MapUnit mapUnit)
    {
        List<int> units = new();

        for(int i = 0; i < mapUnit.units.Count; i++)
        {
            units.Add(i);
        }

        return units;
    }

    public static void ExecuteAction(CombatAction action, Affiliation affiliation, int unitID)
    {
        foreach(var subAction in action.subActions)
        {
            Army target = SelectTarget(subAction, affiliation);

            int multiplier = 0;

            switch(subAction.quantity)
            {
                case Quantity.Single:
                    multiplier = 1;
                    break;
				case Quantity.AoE:
                    multiplier = target.mapUnit.units.Count;
					break;
			}

            switch(subAction.effect)
            {
                case Effect.Damage:
                    if (!OtherArmy(target).disarmed.Contains(unitID))
                    {
                        for(int i = 0; i < multiplier; i++)
                            target.TakeDamage(subAction.value);
                    }
                    break;
				case Effect.Heal:
					target.HealHealth(subAction.value * multiplier);
					break;
				case Effect.Block:
					target.ChangeBlock(subAction.value * multiplier);
					break;
				case Effect.Disarm:
                    target.AddCombatStatus(CombatStatus.Disarm, subAction.value * multiplier);
					break;
				case Effect.Burn:
					target.AddCombatStatus(CombatStatus.Burn, subAction.value * multiplier);
					break;
				case Effect.Freeze:
					target.AddCombatStatus(CombatStatus.Freeze, subAction.value * multiplier);
					break;
				case Effect.Poison:
					target.AddCombatStatus(CombatStatus.Poison, subAction.value * multiplier);
					break;
				case Effect.Haste:
					target.AddCombatStatus(CombatStatus.Haste, subAction.value * multiplier);
					break;
				case Effect.Shatter:
					target.ChangeBlock(subAction.value * multiplier);
					break;
				case Effect.Wound:
					target.ChangeWounds(subAction.value * multiplier);
					break;
			}
        }

		CombatPanel.Instance.UpdateStats();

        if (affiliation == Affiliation.Player)
            CheckForPlayerTurnEnd();
	}

    static Army SelectTarget(SubAction subAction, Affiliation affiliation)
    {
		switch (affiliation)
		{
			case Affiliation.Player:
                if (subAction.target == Target.Ally)
                    return playerArmy;
                else
                    return opponentArmy;
			default:
				if (subAction.target == Target.Opponent)
					return playerArmy;
				else
					return opponentArmy;
		}
	}

    static Army OtherArmy(Army army)
    {
        if (army.mapUnit.affiliation == Affiliation.Player)
            return opponentArmy;
        else
            return playerArmy;
    }
}
