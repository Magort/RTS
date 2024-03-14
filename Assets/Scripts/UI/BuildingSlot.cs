using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Building building;

    WaitForSeconds popupWaiter = new(0.4f);

    public void PopulateButton(Building building)
    {
        this.building = building;
        image.sprite = building.icon;
        gameObject.SetActive(true);
    }

    public void OnClick()
    {
        if(BuildingHandler.Instance.TryBuild(building))
        {
            BuildingHandler.Instance.PopulateBuildingsList(true);
			AudioManager.Instance.Play(Sound.Name.Click);
			AudioManager.Instance.Play(Sound.Name.Build);
		}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(PopulateHoverPanel());
    }

    IEnumerator PopulateHoverPanel()
    {
        yield return popupWaiter;

        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary
                || (touch.phase == TouchPhase.Stationary && touch.deltaPosition.magnitude < 10))

				HoverPanel.instance.PopulateHoverPanel
                    (gameObject, building._name, building.Description() + "\n" + building.RequirementsToString());
        }
	}

	public void OnPointerExit(PointerEventData eventData)
    {
        HoverPanel.instance.DepopulateHoverPanel();
    }
}
