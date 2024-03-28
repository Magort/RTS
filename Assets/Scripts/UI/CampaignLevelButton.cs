using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignLevelButton : MonoBehaviour
{
	public LevelData levelData;

	public void PopulateButton(LevelData levelData)
	{
		this.levelData = levelData;
		gameObject.SetActive(true);
	}

	public void DepopulateButton()
	{
		levelData = null;
		gameObject.SetActive(false);
	}

	public void LoadLevel()
	{
		StartCoroutine(LoadScene());
	}

	IEnumerator LoadScene()
	{
		var asyncLoad = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Additive);

		while(!asyncLoad.isDone)
		{
			yield return null;
		}
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
		GameManager.Instance.LoadLevel(levelData);
		SceneManager.UnloadSceneAsync("MainMenu");
	}
}
