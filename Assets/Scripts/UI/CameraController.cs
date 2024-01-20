using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.1f, 0.3f)] public float intensity;
    public float minZoom;
    public float maxZoom;
    public float currentZoom;
    public bool tapLocked;

    Vector3 moveToTileOffset = new(0, 0, -1.5f);
    float lastTouchDistance = 0;

    WaitForSeconds tapLockWaiter = new(0.1f);
    float tapLockTimer = 0;
    float tapLockTime = 0.2f;

	public static CameraController Instance;

    private void Start()
    {
        Instance = this;
        currentZoom = transform.position.y;
    }

    private void Update()
    {
        if(Input.touchCount == 0)
            return;

        if (Input.touchCount >= 2)
        {
            UpdateZoom();
            return;
        }

        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved)
        {
            transform.position -= intensity * Time.deltaTime * new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y);

            if(!tapLocked)
            {
                StartCoroutine(TapLock());
            }
            else
            {
                tapLockTimer = 0;
            }
        }
    }

    void UpdateZoom()
    {
        var touch1 = Input.GetTouch(0);
        var touch2 = Input.GetTouch(1);

        if (touch1.phase != TouchPhase.Moved && touch2.phase != TouchPhase.Moved)
            return;

		if (touch1.deltaPosition.x * touch2.deltaPosition.x >= 0)
            return;

        var currentDistance = Vector2.Distance(touch1.position, touch2.position);

		currentZoom = Mathf.Clamp(currentZoom + lastTouchDistance - currentDistance, minZoom, maxZoom);

		var tf = transform;
		var pos = tf.position;
		pos = new Vector3(pos.x, Mathf.Lerp(pos.y, currentZoom, Time.deltaTime * 5), pos.z);
		tf.position = pos;

		lastTouchDistance = Vector2.Distance(touch1.position, touch2.position);
	}

    public void MoveToTile(Vector3 spot)
    {
        StartCoroutine(MoveTo(spot + moveToTileOffset));
    }

    IEnumerator TapLock()
    {
        tapLocked = true;

        tapLockTimer = 0;

        while(tapLockTimer < tapLockTime)
        {
            yield return tapLockWaiter;

            tapLockTimer += 0.1f;
        }

        tapLocked = false;
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
}
