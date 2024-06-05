using UnityEngine;

[System.Serializable]
public class Narrator
{
	public Sprite avatar;
	public string name;
	public Code code;

	public enum Code
	{
		Tutorial,
		Hiro
	}
}
