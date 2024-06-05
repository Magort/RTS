using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
	public List<ProgressBar> progressBars;

	public static ProgressBarManager Instance;

	private void Start()
	{
		Instance = this;
	}

	public ProgressBar GetProgressBar()
	{
		return progressBars.Find(bar => !bar.gameObject.activeSelf);
	}
}
