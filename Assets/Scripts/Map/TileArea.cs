using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileArea : MonoBehaviour
{
    public static List<int> TypeChances = new() { 70, 82, 94, 98, 100 };
	public static int MaxSameType = 3;

	public Type type;

	public GameObject woodDecorations;
	public GameObject foodDecorations;
	public GameObject goldDecorations;
	public GameObject essenceDecorations;
    public GameObject buildingSlot;

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

    public void ShowDecorations()
    {
        if(type != Type.Empty)
        {
            typeToDecoration[type].SetActive(true);
        }
    }

    public void HideDecorations()
    {
		typeToDecoration[type].SetActive(false);
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
