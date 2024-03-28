using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatPanel : MonoBehaviour
{
	public static CombatPanel Instance;

	public List<TextMeshProUGUI> playerUnits;
	public List<TextMeshProUGUI> opponentUnits;

	public List<ActionSlot> playerActions;
	public List<ActionSlot> opponentActions;

	public TextMeshProUGUI playerHealthText;
	public TextMeshProUGUI opponentHealthText;
	public TextMeshProUGUI playerArmiesText;
	public TextMeshProUGUI opponentArmiesText;
	public Image playerHealthBar;
	public Image opponentHealthBar;
	public List<StatusSlot> playerStatuses;
	public List<StatusSlot> opponentStatuses;

	public Button rollButton;
	public GameObject combatStartPrompt;
	public SpeedPrompt speedPrompt;
	public CombatEndPrompt combatEndPrompt;

	Affiliation looser = Affiliation.Enemy;

	private void Start()
	{
		Instance = this;
		gameObject.SetActive(false);
	}

	public void SwitchCombatStartPromptState(bool state)
	{
		combatStartPrompt.SetActive(state);
	}

	public void ShowCombatEndPrompt(Affiliation looser)
	{
		this.looser = looser;
		combatEndPrompt.ShowPrompt(looser);
	}

	public void PopulatePanel()
	{
		gameObject.SetActive(true);
		ClearActions();
		ShowUnits();
		SwitchRollButton(false);
		UpdateStats();
	}

	public void HidePanel()
	{
		CombatHandler.ResolvePostCombatLoses(looser);
		gameObject.SetActive(false);
	}	

	public void OnRollButtonClick()
	{
		AudioManager.Instance.Play(Sound.Name.Click);
		CombatHandler.StartNewRound();
		SwitchRollButton(false);
	}

	public void SwitchRollButton(bool status)
	{
		rollButton.interactable = status;
	}

	public void ShowUnits()
	{
		ClearUnits();

		for (int i = 0; i < CombatHandler.playerArmy.mapUnit.units.Count; i++)
		{
			playerUnits[i].text = CombatHandler.playerArmy.mapUnit.units[i].unitName;
		}

		for (int i = 0; i < CombatHandler.opponentArmy.mapUnit.units.Count; i++)
		{
			opponentUnits[i].text = CombatHandler.opponentArmy.mapUnit.units[i].unitName;
		}
	}

	void ClearUnits()
	{
		foreach (TextMeshProUGUI unit in playerUnits)
		{
			unit.text = "";
		}
		foreach (TextMeshProUGUI unit in opponentUnits)
		{
			unit.text = "";
		}
	}

	public void UpdateStats()
	{
		playerHealthText.text = CombatHandler.playerArmy.health.ToString();
		playerHealthBar.fillAmount = (float)CombatHandler.playerArmy.health / (float)CombatHandler.playerArmy.maxHealth;
		playerArmiesText.text = CombatHandler.tileFoughtOn.data.units
			.Where(unit => unit.affiliation == Affiliation.Player).ToList().Count.ToString()
			+ "<sprite=" + IconIDs.quantityToIconID[(CombatAction.Quantity.Single, CombatAction.Target.Ally)] + ">";
		foreach (StatusSlot status in playerStatuses)
		{
			status.text.text = CombatHandler.playerArmy.GetStatusAmount(status.status).ToString();
		}

		opponentHealthText.text = CombatHandler.opponentArmy.health.ToString();
		opponentHealthBar.fillAmount = (float)CombatHandler.opponentArmy.health / (float)CombatHandler.opponentArmy.maxHealth;
		opponentArmiesText.text = CombatHandler.tileFoughtOn.data.units
			.Where(unit => unit.affiliation != Affiliation.Player).ToList().Count.ToString()
			+ "<sprite=" + IconIDs.quantityToIconID[(CombatAction.Quantity.Single, CombatAction.Target.Opponent)] + ">";
		foreach (StatusSlot status in opponentStatuses)
		{
			status.text.text = CombatHandler.opponentArmy.GetStatusAmount(status.status).ToString();
		}
	}

	public void ShowActionRolls(Affiliation affiliation)
	{
		StartCoroutine(ShowActionRollsCoroutine(affiliation));
	}

	IEnumerator ShowActionRollsCoroutine(Affiliation affiliation)
	{
		var waiter = new WaitForSecondsRealtime(0.33f);

		switch(affiliation)
		{
			case Affiliation.Player:
				playerActions.ForEach(action => action.ClearActions());
				var playerRolls = CombatHandler.RollActions(CombatHandler.playerArmy);

				int playerActionIndex = 0;

				for (int unitID = 0; unitID < CombatHandler.playerArmy.mapUnit.units.Count; unitID++)
				{
					if (CombatHandler.playerArmy.frozen.Contains(unitID))
						continue;

					playerActions[unitID].AssignActions(playerRolls[playerActionIndex], affiliation);

					if (CombatHandler.playerArmy.hasted.Contains(unitID))
					{
						playerActionIndex++;
						playerActions[unitID].AssignBonusActions(playerRolls[playerActionIndex], affiliation);
					}

					playerActionIndex++;

					yield return waiter;
				}
				break;

			default:
				opponentActions.ForEach(action => action.ClearActions());
				var opponentRolls = CombatHandler.RollActions(CombatHandler.opponentArmy);

				int opponentActionIndex = 0;

				for (int unitID = 0; unitID < CombatHandler.opponentArmy.mapUnit.units.Count; unitID++)
				{
					if (CombatHandler.opponentArmy.frozen.Contains(unitID))
						continue;

					opponentActions[unitID].AssignActions(opponentRolls[opponentActionIndex], affiliation);

					if (CombatHandler.opponentArmy.hasted.Contains(unitID))
					{
						opponentActionIndex++;
						opponentActions[unitID].AssignBonusActions(opponentRolls[opponentActionIndex], affiliation);
					}

					opponentActionIndex++;
					yield return waiter;
				}

				StartCoroutine(ResolveEnemyActions());
				break;
		}
	}

	void ClearActions()
	{
		opponentActions.ForEach(action => action.ClearActions());
		playerActions.ForEach(action => action.ClearActions());
	}

	IEnumerator ResolveEnemyActions()
	{
		yield return new WaitForSecondsRealtime(1);

		foreach (var action in opponentActions)
		{
			yield return new WaitForSecondsRealtime(action.HandleEnemyActions() / 2);
		}

		CombatHandler.EndRound();
	}

	public void ShowAnimations(CombatAction action)
	{
		//StartCoroutine(ShowAnimationsCoroutine(action));
	}

	IEnumerator ShowAnimationsCoroutine(CombatAction action)
	{
		var animationWaiter = new WaitForSecondsRealtime(0.5f);

		foreach (var subAction in action.subActions)
		{

		}

		yield return null;
	}

	public void GoToFight()
	{
		CameraController.Instance.MoveToTile(CombatHandler.tileFoughtOn.transform.position);
	}

	public void StartCombat()
	{
		CombatHandler.PrepareCombat();
		SwitchCombatStartPromptState(false);
	}
	public void StartFirstRound()
	{
		CombatHandler.StartFirstRound();
		speedPrompt.gameObject.SetActive(false);
	}

	public void PlayClickSound()
	{
		AudioManager.Instance.Play(Sound.Name.Click);
	}
}
