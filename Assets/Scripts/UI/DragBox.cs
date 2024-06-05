using TMPro;
using UnityEngine;

public class DragBox : MonoBehaviour
{
	public static DragBox Instance;
	public TextMeshProUGUI textBox;
	Vector3 offset = new(0, -50, 0);

	private void Awake()
	{
		Instance = this;
		gameObject.SetActive(false);
	}

	public void EnableDragBox(string text)
	{
		textBox.text = text;
		transform.position = Input.mousePosition + offset;
		gameObject.SetActive(true);
	}
	public void DisableDragBox()
	{
		gameObject.SetActive(false);
	}

	void Update()
	{
		transform.position = Input.mousePosition + offset;
	}
}
