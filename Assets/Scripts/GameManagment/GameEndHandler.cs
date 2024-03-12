using UnityEngine;

public class GameEndHandler : MonoBehaviour
{
    public static GameEndHandler Instance;
    
    public GameEndPrompt prompt;

    private void Start()
    {
        Instance = this;
    }

    public void WinGame()
    {
        prompt.PopulatePrompt(true);
    }

    public void LoseGame()
    {
		prompt.PopulatePrompt(false);
	}

	public void CloseGame()
	{
		Application.Quit();
	}
}
