using TMPro;
using UnityEngine;

public class HoverPanel : MonoBehaviour
{
    public static HoverPanel instance;

    public TextMeshProUGUI title;
    public TextMeshProUGUI body;

    int screenEdgeOffset = 15;

    Vector3 offset = new(0, 175, 0);

    GameObject target;

    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!target) return;

        if (!target.activeInHierarchy)
        {
            gameObject.SetActive(false);
            target = null;
        }
    }

    public void PopulateHoverPanel(GameObject target, string titleText, string bodyText)
    {
        this.target = target;
        gameObject.SetActive(true);
        gameObject.transform.position = target.transform.position + offset;
        SetInScreenBounds();

        title.text = titleText;
        body.text = bodyText;
    }

    public void DepopulateHoverPanel()
    {
		title.text = "";
		body.text = "";

        target = null;
        gameObject.SetActive(false);
	}

    void SetInScreenBounds()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform.position.x + (rectTransform.rect.width / 2) > Camera.main.scaledPixelWidth)
        {
            transform.position = new(Camera.main.scaledPixelWidth - (rectTransform.rect.width / 2) - screenEdgeOffset, transform.position.y, transform.position.z);
        }
        if (rectTransform.position.x - (rectTransform.rect.width / 2) < 0)
        {
            transform.position = new(0 + (rectTransform.rect.width / 2) + screenEdgeOffset, transform.position.y, transform.position.z);
		}
    }
}
