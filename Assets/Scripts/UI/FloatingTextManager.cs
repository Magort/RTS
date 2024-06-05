using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
	public GameObject textContainer;
	public GameObject textPrefab;

	private List<FloatingText> floatingTexts = new();

	public static FloatingTextManager Instance;

	public void Show(string msg, Color color, Vector3 position, Vector3 motion, float duration)
	{
		FloatingText txt = GetFloatingText();

		txt.text.text = msg;
		txt.text.color = color;
		position.z = -1;
		txt.gObject.transform.position = new Vector3(position.x, position.y, -3);
		txt.motion = motion;
		txt.duration = duration;

		txt.Show();
	}

	private FloatingText GetFloatingText()
	{
		FloatingText txt = floatingTexts.Find(t => !t.isActive);

		if (txt == null)
		{
			txt = new FloatingText();
			txt.gObject = Instantiate(textPrefab);
			txt.gObject.transform.SetParent(textContainer.transform);
			txt.text = txt.gObject.GetComponent<TextMeshProUGUI>();

			floatingTexts.Add(txt);
		}

		return txt;
	}

	private void CreateObject()
	{
		FloatingText txt;
		txt = new FloatingText();
		txt.gObject = Instantiate(textPrefab);
		txt.gObject.transform.SetParent(textContainer.transform);
		txt.text = txt.gObject.GetComponent<TextMeshProUGUI>();

		floatingTexts.Add(txt);
	}

	private void Start()
	{
		Instance = this;

		for (int i = 0; i < 30; i++)
		{
			CreateObject();
			floatingTexts[i].gObject.name += i;
		}
	}

	private void Update()
	{
		foreach (FloatingText text in floatingTexts)
		{
			text.UpdateFloatingText();
		}
	}
}
