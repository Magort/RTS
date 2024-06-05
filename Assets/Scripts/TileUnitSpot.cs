using System.Collections.Generic;
using UnityEngine;

public class TileUnitSpot : MonoBehaviour
{
	public Tile tile;
	public GameObject unitModel;
	public MeshRenderer hatMeshRenderer;
	[Header("Materials")]
	public Material playerMaterial;
	public Material enemyMaterial;
	public Material neutralMaterial;

	Dictionary<Affiliation, Material> affiliationToMaterial;

	private void Awake()
	{
		affiliationToMaterial = new()
		{
			{ Affiliation.Player, playerMaterial },
			{ Affiliation.Enemy, enemyMaterial },
			{ Affiliation.Neutral, neutralMaterial }
		};
	}

	public void ShowUnitModel(Affiliation affiliation)
	{
		if (!tile.data.discovered)
			return;

		unitModel.SetActive(true);
		hatMeshRenderer.material = affiliationToMaterial[affiliation];
	}

	public void HideUnitModel()
	{
		unitModel.SetActive(false);
	}
}
