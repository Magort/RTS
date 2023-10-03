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
        foodText.text = "Food: "
			+ GameState.Resources[Resource.Food].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Food]);
		woodText.text = "Wood: "
			+ GameState.Resources[Resource.Wood].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Wood]);
		goldText.text = "Gold: "
			+ GameState.Resources[Resource.Gold].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Gold]);
		essenceText.text = "Essence: "
			+ GameState.Resources[Resource.Essence].ToString()
			+ " "
			+ GetGrowthText(GameState.ResourcesGrowth[Resource.Essence]);
	}

	string GetGrowthText(int value)
	{
		if (value < 0)
			return "<color=red>" + value + "</color>";
		if(value > 0)
			return "<color=green>+" + value + "</color>";


		return "<color=black>+" + value + "</color>";
	}
}
