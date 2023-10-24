using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileArea : MonoBehaviour
{
    public static List<int> TypeChances = new() { 70, 82, 94, 98, 100 };
    public static Dictionary<Type, int> MaxSameType = new()
    {
        {Type.Empty, 6 },
		{Type.WoodSource, 3 },
		{Type.FoodSource, 3 },
		{Type.GoldSource, 2 },
		{Type.EssenceSource, 1 }
	};
    public static Dictionary<Type, int> ResourceStartingAmount = new()
    {
		{Type.Empty, 0 },
		{Type.WoodSource, 100 },
		{Type.FoodSource, 100 },
		{Type.GoldSource, 40 },
		{Type.EssenceSource, 10000 }
	};

	public Type type;
    public int resourceAmount = 0;
    public Building.Code building;

	public GameObject woodDecorations;
	public GameObject foodDecorations;
	public GameObject goldDecorations;
	public GameObject essenceDecorations;
    public GameObject buildingSlot;
    public LineRenderer lineRenderer;

    Dictionary<Type, GameObject> typeToDecoration;

    private void Awake()
    {
        typeToDecoration = new()
        {
			{Type.WoodSource, woodDecorations },
			{Type.FoodSource, foodDecorations },
			{Type.GoldSource, goldDecorations },
			{Type.EssenceSource, essenceDecorations },
			{Type.Building, buildingSlot }
		};
    }

    public void DepleteResources()
    {
		HideDecorations();
		type = Type.Empty;
    }

    public void RemoveBuilding()
    {
        Destroy(buildingSlot.transform.GetChild(0).gameObject);
        type = Type.Empty;
    }

    public void ShowDecorations()
    {
        if(type != Type.Empty)
        {
            typeToDecoration[type].SetActive(true);
        }
    }

    public void HideDecorations()
    {
        if (type != Type.Empty)
        {
            typeToDecoration[type].SetActive(false);
        }
	}

    public enum Type
    {
        Empty,
        WoodSource,
        FoodSource,
        GoldSource,
        EssenceSource,
		Building
	}
}
