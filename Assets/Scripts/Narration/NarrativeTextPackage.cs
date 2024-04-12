using UnityEngine;

[System.Serializable]
public class NarrativeTextPackage
{
    public Narrator.Code narrator;
    [TextArea(6, 10)] public string content;
}
