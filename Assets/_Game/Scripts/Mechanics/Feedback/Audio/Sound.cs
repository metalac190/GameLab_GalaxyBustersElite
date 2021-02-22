using UnityEngine;

[System.Serializable]
public class Sound
{
    // Inspector name
    [HideInInspector] public string name = "Sound";



    public AudioClip audioClip;

    [Space(10)]
    [Range(0, 1)]
    public float volume = 0.5f;
    public bool playOnAwake = false;
    public bool loop = false;

    [HideInInspector] public AudioSource audioSource;
}