using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    [Range(0.1f, 1f)]
    public float volume;
    public bool loop;
	public Name name;

	private float pitch = 1;

    public void Initialize(AudioSource source)
    {
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.clip = clip;
        source.playOnAwake = false;
	}

    public enum Name
    {
        MainMusic,
        CombatDamage,
        CombatHeal,
        CombatDisarm,
        CombatPoison,
        CombatBurn,
        CombatCrush,
        CombatFreeze,
        CombatWound,
        CombatBlock,
        Click,
        Build,
        CombatHaste
    }
}
