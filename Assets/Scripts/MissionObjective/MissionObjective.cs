using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MissionObjective : ScriptableObject
{
    public int quantityToPass;
    public int timeToPass;
    public string description;
    public void ProgressQuantity()
    {

    }    
    public abstract bool ConditionsMet();
    public abstract void SubscribeToEvents();
}
