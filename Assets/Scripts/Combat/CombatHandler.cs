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

        public List<CombatStatus> statuses;
        public MapUnit mapUnit;

        public List<int> frozen;
		public List<int> disarmed;
		public List<int> hasted;

		int maxBlock = 10;

		public void InitializeArmy(MapUnit mapUnit)
        {
            this.mapUnit = mapUnit;
            maxHealth = mapUnit.GetHealthSum();
            health = mapUnit.GetHealthSum();
            speed = mapUnit.GetSpeedSum();
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
        }
		public void TakeDamage(int value)
        {
            health -= Mathf.Max(0, value - GetStatusAmount(CombatStatus.Block) + GetStatusAmount(CombatStatus.Wound));
        }
        public void TakeDamageNoWounds(int value)
        {
            health -= value;
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
            if (value > 0)
            {
                for (int i = 0; i < value; i++)
                {
                    if (GetStatusAmount(CombatStatus.Block) < maxBlock)
                        AddCombatStatus(CombatStatus.Block, 1);
                    else
                        break;
                }
            }
            else
            {
				for (int i = 0; i < Mathf.Abs(value); i++)
				{
                    if (GetStatusAmount(CombatStatus.Block) > 0)
                        RemoveCombatStatus(CombatStatus.Block, 1);
                    else
                        break;
				}
			}
        }
        public void ChangeWounds(int value)
        {
			if (value > 0)
			{
				for (int i = 0; i < value; i++)
				{
					AddCombatStatus(CombatStatus.Wound, 1);
				}
			}
			else
			{
				for (int i = 0; i < Mathf.Abs(value); i++)
				{
					if (GetStatusAmount(CombatStatus.Wound) > 0)
						RemoveCombatStatus(CombatStatus.Wound, 1);
					else
						break;
				}
			}
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
			GameManager.SwitchPauseState(true);
            tileFoughtOn = tile;
			CombatPanel.Instance.SwitchCombatStartPromptState(true);
        }
    }

    public static void CheckForPlayerTurnEnd()
    {
        actionsMade++;

        if (actionsMade >= maxPlayerActions)
            HandleEnemyTurn();
    }

    public static void PrepareCombat()
    {
        GameManager.SwitchPauseState(true);

        presentMapUnits = new();

        foreach (MapUnit unit in tileFoughtOn.units)
        {
            presentMapUnits.Add(unit);
        }    

        presentMapUnits.OrderByDescending(army => army.units.Count).ThenByDescending(army => army.GetSpeedSum());

        playerArmy = new();
        playerArmy.InitializeArmy(GetNextMapUnit(Affiliation.Player));

        opponentArmy = new();
        opponentArmy.InitializeArmy(GetNextMapUnit(Affiliation.Enemy));

		CombatPanel.Instance.PopulatePanel();
        UnitMovementHandler.Instance.Deselect();

        if(playerArmy.speed < opponentArmy.speed)
        {
            CombatPanel.Instance.speedPrompt.PopulateSpeedPrompt(Affiliation.Enemy);
        }
        else
        {
            CombatPanel.Instance.speedPrompt.PopulateSpeedPrompt(Affiliation.Player);
        }    
	}

    public static void StartFirstRound()
    {
		CombatPanel.Instance.SwitchRollButton(false);

		if (playerArmy.speed < opponentArmy.speed)
		{
            HandleEnemyTurn();
		}
		else
		{
            StartNewRound();
		}
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
            if(army.mapUnit.affiliation == Affiliation.Player)
            {
                List<Unit> toRemove = new();
                foreach(var unit in army.mapUnit.units)
                {
                    toRemove.Add(unit);
                }
                foreach(Unit unit in toRemove)
                {
					army.mapUnit.KillUnit(unit);
				}
            }

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
            EndCombat(affiliation);
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

    static void EndCombat(Affiliation looser)
    {
        CombatPanel.Instance.ShowCombatEndPrompt(looser);

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

        army.TakeDamageNoWounds(army.GetStatusAmount(CombatStatus.Poison));
        army.TakeDamageNoWounds(army.GetStatusAmount(CombatStatus.Burn) * army.mapUnit.units.Count);

        army.RemoveCombatStatus(CombatStatus.Burn, Mathf.RoundToInt(army.GetStatusAmount(CombatStatus.Burn) * 0.75f));
		army.RemoveCombatStatus(CombatStatus.Block, army.GetStatusAmount(CombatStatus.Block));

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

    public static void ExecuteSubAction(SubAction subAction, Affiliation affiliation, int unitID)
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

		switch (subAction.effect)
		{
			case Effect.Damage:
				if (!OtherArmy(target).disarmed.Contains(unitID))
				{
					for (int i = 0; i < multiplier; i++)
						target.TakeDamage(subAction.value);
				}
				AudioManager.Instance.Play(Sound.Name.CombatDamage);
				break;
			case Effect.Heal:
				target.HealHealth(subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatHeal);
				break;
			case Effect.Block:
				target.ChangeBlock(subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatBlock);
				break;
			case Effect.Disarm:
				target.AddCombatStatus(CombatStatus.Disarm, subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatDisarm);
				break;
			case Effect.Burn:
				target.AddCombatStatus(CombatStatus.Burn, subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatBurn);
				break;
			case Effect.Freeze:
				target.AddCombatStatus(CombatStatus.Freeze, subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatFreeze);
				break;
			case Effect.Poison:
				target.AddCombatStatus(CombatStatus.Poison, subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatPoison);
				break;
			case Effect.Haste:
				target.AddCombatStatus(CombatStatus.Haste, subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatHaste);
				break;
			case Effect.Shatter:
				target.ChangeBlock(-subAction.value * multiplier);
				break;
			case Effect.Wound:
				target.ChangeWounds(subAction.value * multiplier);
				AudioManager.Instance.Play(Sound.Name.CombatWound);
				break;
		}

		CombatPanel.Instance.UpdateStats();
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
