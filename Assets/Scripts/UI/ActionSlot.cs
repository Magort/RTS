using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSlot : MonoBehaviour
{
	List<CombatAction> combatActions;
	public Affiliation affiliation;

	[Header("Refs")]
	public List<ActionButton> actionButtons;

	WaitForSecondsRealtime waiter = new(0.25f);

	public void AssignActions((CombatAction, CombatAction) actions, Affiliation affiliation)
	{
		ClearActions();

		combatActions.Add(actions.Item1);
		combatActions.Add(actions.Item2);

		for (int i = 0; i < 2; i++)
		{
			actionButtons[i].PopulateButton(combatActions[i], affiliation);
		}
	}

	public void AssignBonusActions((CombatAction, CombatAction) actions, Affiliation affiliation)
	{
		combatActions.Add(actions.Item1);
		combatActions.Add(actions.Item2);

		for (int i = 0; i < 2; i++)
		{
			actionButtons[i + 2].PopulateButton(combatActions[i + 2], affiliation);
		}
	}

	public void ClearActions()
	{
		actionButtons.ForEach(button => button.HideButton());
		combatActions = new();
	}

	public void OnClick(int index)
	{
		StartCoroutine(ExecuteSubActions(combatActions[index]));
		CombatPanel.Instance.ShowAnimations(combatActions[index]);
		DisablePairActions(index);

		FloatingTextManager.Instance
			.Show(combatActions[index].name, Color.white, actionButtons[index].transform.position, new Vector3(0, 50, 0), 0.75f);
	}

	IEnumerator ExecuteSubActions(CombatAction action)
	{
		for (int i = 0; i < action.subActions.Count; i++)
		{
			CombatHandler.ExecuteSubAction(action.subActions[i], affiliation, transform.GetSiblingIndex());
			yield return waiter;
		}

		if (affiliation == Affiliation.Player)
			CombatHandler.CheckForPlayerTurnEnd();
	}

	void DisablePairActions(int index)
	{
		switch (index)
		{
			case 0:
				actionButtons[0].SetInteractable(false);
				actionButtons[1].HideButton();
				break;
			case 1:
				actionButtons[1].SetInteractable(false);
				actionButtons[0].HideButton();
				break;
			case 2:
				actionButtons[2].SetInteractable(false);
				actionButtons[3].HideButton();
				break;
			case 3:
				actionButtons[3].SetInteractable(false);
				actionButtons[2].HideButton();
				break;
		}
	}

	public int HandleEnemyActions()
	{
		StartCoroutine(HandleActions(combatActions.Count));
		return combatActions.Count;
	}

	IEnumerator HandleActions(int amount)
	{
		WaitForSecondsRealtime waiter = new(1);
		int min = 0;
		int max = 2;

		for (int i = 0; i < amount / 2; i++)
		{
			OnClick(UnityEngine.Random.Range(min, max));
			min += 2;
			max += 2;
			yield return waiter;
		}
	}
}
