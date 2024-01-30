using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Image image;

    public Sprite playSprite;
    public Sprite pauseSprite;

    Dictionary<float, Sprite> pauseStateToImage;

	private void Start()
	{
		pauseStateToImage = new()
        {
            { 0, playSprite },
            { 1, pauseSprite }
        };
	}
	public void SwitchPauseState()
    {
        GameManager.SwitchPauseState(Time.timeScale == 1);
        image.sprite = pauseStateToImage[Time.timeScale];
    }    
}
