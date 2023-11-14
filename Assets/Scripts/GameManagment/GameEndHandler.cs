using UnityEngine;

public class GameEndHandler : MonoBehaviour
{
    public static GameEndHandler Instance;
    
    public GameEndPrompt prompt;

    int winConCounter = 0;
    int winConRequired = 3;

    private void Start()
    {
        Instance = this;
    }

    public void ProgressWinCon()
    {
        winConCounter++;
        if (winConCounter >= winConRequired)
            WinGame();
    }

    void WinGame()
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
