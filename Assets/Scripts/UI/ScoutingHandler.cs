using System.Collections;
using UnityEngine;

public class ScoutingHandler : MonoBehaviour
{
    public int scoutingTime;
    WaitForSeconds waiter = new(1);

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void TryScout()
    {
        if (GameState.ScoutsAvailable > 0)
        {
            ContextMenu.Instance.StartCoroutine(StartScouting(ContextMenu.Instance.SelectedTile));
            ContextMenu.Instance.CloseAll();
        }
    }

    IEnumerator StartScouting(Tile scoutedTile)
    {
        GameState.ScoutsAvailable--;

        //Show update bar
        float timer = 0;
        while(timer < scoutingTime)
        {
            yield return waiter;
            timer++;
        }

        scoutedTile.Reveal();

		GameState.ScoutsAvailable++;
	}
}
