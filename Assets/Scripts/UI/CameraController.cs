using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.25f, 1)] public float intensity;
	public bool dragged;
    public float minZoom;
    public float maxZoom;
    public float currentZoom;

	bool dragging;

    WaitForSeconds draggedWaiter = new(0.4f);

    Vector3 lastMousePos = new();
    Vector3 moveToTileOffset = new(0, 0, -1.5f);

    public static CameraController Instance;

    private void Start()
    {
        Instance = this;
        currentZoom = transform.position.y;
    }

    private void Update()
    {
        UpdateZoom();

        if (Input.GetMouseButtonDown(1))
        {
            dragging = true;
            lastMousePos = Input.mousePosition;
        }

		if (Input.GetMouseButtonUp(1))
            dragging = false;

        if (!dragging)
            return;

        Camera.main.transform.position
            -= (new Vector3(Input.mousePosition.x - lastMousePos.x, 0, Input.mousePosition.y - lastMousePos.y) / 100 )
            * intensity * Time.timeScale;

        if(!dragged)
        {
            if(Input.mousePosition != lastMousePos)
            {
                StartCoroutine(Dragged());
            }
        }

        lastMousePos = Input.mousePosition;
    }

    void UpdateZoom()
    {
		currentZoom = Mathf.Clamp(currentZoom - Input.GetAxis("Mouse ScrollWheel"), minZoom, maxZoom);

		var tf = transform;
		var pos = tf.position;
		pos = new Vector3(pos.x, Mathf.Lerp(pos.y, currentZoom, Time.deltaTime * 5), pos.z);
		tf.position = pos;
	}

    public void MoveToTile(Vector3 spot)
    {
        StartCoroutine(MoveTo(spot + moveToTileOffset));
    }

    IEnumerator MoveTo(Vector3 spot)
    {
        Vector3 startingPosition = transform.position;
        float step = 0;
        var waiter = new WaitForSecondsRealtime(0.01f);
        spot.y = startingPosition.y;

        while(step < 1)
        {
            transform.position = Vector3.Lerp(startingPosition, spot, step);
            step += 0.01f;
            yield return waiter;
        }
    }

    IEnumerator Dragged()
    {
        dragged = true;

        yield return draggedWaiter;

        dragged = false;
    }    
}
