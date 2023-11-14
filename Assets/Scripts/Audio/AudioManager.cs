using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<Sound> sounds;

    private Dictionary<Sound.Name, AudioSource> nameToSource = new();

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(var sound in sounds)
        {
            var source = gameObject.AddComponent<AudioSource>();
			sound.Initialize(source);
			nameToSource.Add(sound.name, source);
		}
    }

    public void Play(Sound.Name name)
    {
        nameToSource[name].Play();
    }
}
