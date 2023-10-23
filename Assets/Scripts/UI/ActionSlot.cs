using System.Collections.Generic;
using UnityEngine;

public class ActionSlot : MonoBehaviour
{
    List<CombatAction> combatActions;
	public Affiliation affiliation;

	[Header("Refs")]
	public List<ActionButton> actionButtons;

	public void AssignActions((CombatAction, CombatAction) actions, Affiliation affiliation)
    {
		ClearActions();

		combatActions.Add(actions.Item1);
		combatActions.Add(actions.Item2);

        for(int i = 0; i < 2; i++)
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
		CombatHandler.ExecuteAction(combatActions[index], affiliation, transform.GetSiblingIndex());
		CombatPanel.Instance.ShowAnimations(combatActions[index]);
		DisablePairActions(index);

		FloatingTextManager.Instance
			.Show(combatActions[index].name, Color.white, actionButtons[index].transform.position, new Vector3(0, 50, 0), 0.75f);
	}

	void DisablePairActions(int index)
	{
		switch(index)
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

	public bool HandleEnemyAction()
	{
		if (combatActions.Count == 0)
			return false;

		OnClick(Random.Range(0, 2));

		if (combatActions.Count <= 2)
			return true;

		OnClick(Random.Range(2, 4));

		return true;
	}
}
