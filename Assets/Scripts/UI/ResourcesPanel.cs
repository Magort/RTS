using System;
using TMPro;
using UnityEngine;

public class ResourcesPanel : MonoBehaviour
{
	public static ResourcesPanel Instance;
    public TextMeshProUGUI foodText;
	public TextMeshProUGUI woodText;
	public TextMeshProUGUI goldText;
	public TextMeshProUGUI essenceText;

	private void Awake()
    {
		Instance = this;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foodText.text = GameState.Resources[Resource.Food].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Food] * 10);

		woodText.text = GameState.Resources[Resource.Wood].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Wood] * 10);

		goldText.text = GameState.Resources[Resource.Gold].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Gold] * 10);

		essenceText.text = GameState.Resources[Resource.Essence].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Essence] * 10);
	}

	string GetGrowthText(float value)
	{
		if (value < 0)
			return "<color=red>" + Math.Round(value, 2) + "</color>";
		if(value > 0)
			return "<color=green>+" + Math.Round(value, 2) + "</color>";


		return "<color=black>+" + value + "</color>";
	}
}
