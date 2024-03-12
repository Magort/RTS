using System.Collections;
using TMPro;
using UnityEngine;

public class ScoutingHandler : MonoBehaviour
{
    public TextMeshProUGUI button;

    public float defaultScoutingTime;
    public string scoutingText;
    WaitForSeconds waiter = new(0.1f);

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void PopulateScoutingButton(bool state)
    {
        gameObject.SetActive(state);
		button.text = "Scout\n(" + GameState.ScoutsAvailable + "/" + GameState.MaxScouts + ")";
	}

    public void TryScout()
    {
		AudioManager.Instance.Play(Sound.Name.Click);

		if (GameState.ScoutsAvailable > 0)
        {
            ContextMenu.Instance.StartCoroutine(StartScouting(ContextMenu.Instance.SelectedTile));
            ContextMenu.Instance.CloseAll();
        }
    }

    public float ScoutingTime()
    {
        var selectedTileCoords = ContextMenu.Instance.SelectedTile.data.navigationCoordinates;
        var mainTileCoords = new Vector3Int(TileGrid.Size, TileGrid.Size, TileGrid.Size * -2);
        return defaultScoutingTime * Mathf.Max(Mathf.Abs(selectedTileCoords.x - mainTileCoords.x),
                         Mathf.Abs(selectedTileCoords.y - mainTileCoords.y),
                         Mathf.Abs(selectedTileCoords.z - mainTileCoords.z));
    }

    IEnumerator StartScouting(Tile scoutedTile)
    {
        GameState.ScoutsAvailable--;
        float scoutingTime = defaultScoutingTime + (ScoutingTime() / 3);

		ProgressBarManager.Instance.GetProgressBar().ShowProgress(scoutedTile.transform, scoutingTime, scoutingText, TileArea.affiliationToColor[Affiliation.Player]);
        scoutedTile.data.beingScouted = true;

		float timer = 0;
        while(timer < scoutingTime)
        {
            yield return waiter;
            timer += 0.1f;
        }

        scoutedTile.Reveal();
        ContextMenu.Instance.UpdatePanel();

		GameState.ScoutsAvailable++;
	}
}
