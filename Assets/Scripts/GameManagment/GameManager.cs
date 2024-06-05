using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public GameObject gameCamera;
	public List<Building> BuildingsList;

	private void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		GameState.AddScouts(2);
		AudioManager.Instance.Play(Sound.Name.MainMusic);
		Application.targetFrameRate = 60;
		GameEventsManager.BuildingCompleted.AddListener(GameState.AddBuilding);
	}

	public void LoadLevel(LevelData levelData)
	{
		MapGenerator.Instance.LoadMap(levelData.tiles);
		ObjectivesManager.Instance.LoadObjectives(levelData.missionObjects);
		BuildingHandler.Instance.LoadAvailableBuildingsList(levelData.availableBuildings);
		gameCamera.transform.position = levelData.cameraStartingPos;
		GrantStartingResources(levelData.startingResources);
	}

	public void GrantStartingResources(List<Building.Requirements.ResourceRequirement> startingResources)
	{
		foreach (var resource in startingResources)
		{
			GameState.AddResource(resource.resource, resource.amount);
		}
	}

	public void ReturnToMainMenu()
	{
		StartCoroutine(SwitchScenes());
	}

	IEnumerator SwitchScenes()
	{
		var asyncLoad = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		SceneManager.UnloadSceneAsync("GameScene");
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
	}

	public static void SwitchPauseState(bool state)
	{
		if (state)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

}
