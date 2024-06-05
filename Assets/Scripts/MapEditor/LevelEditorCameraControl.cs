using UnityEngine;

public class LevelEditorCameraControl : MonoBehaviour
{
	float sensitivity = 2f;

	private void Update()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		var cameraMove = new Vector3(moveHorizontal, 0, moveVertical);
		transform.position += cameraMove * Time.unscaledDeltaTime * sensitivity;
	}
}
