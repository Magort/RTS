using TMPro;
using UnityEngine;

public class SpeedPrompt : MonoBehaviour
{
    public string playerFasterTitle;
	[TextArea] public string playerFasterDescription;
    
    public string enemyFasterTitle;
	[TextArea] public string enemyFasterDescription;

    [Header("References")]
    public TextMeshProUGUI titleTextBox;
    public TextMeshProUGUI descriptionTextBox;
    public TextMeshProUGUI playerSpeed;
    public TextMeshProUGUI enemySpeed;

    public void PopulateSpeedPrompt(Affiliation affiliation)
    {
        gameObject.SetActive(true);

        if(affiliation == Affiliation.Player)
        {
            titleTextBox.text = playerFasterTitle;
            descriptionTextBox.text = playerFasterDescription;
        }
        else
        {
			titleTextBox.text = enemyFasterTitle;
			descriptionTextBox.text = enemyFasterDescription;
		}

        playerSpeed.text = CombatHandler.playerArmy.speed.ToString();
        enemySpeed.text = CombatHandler.opponentArmy.speed.ToString();
    }
}
