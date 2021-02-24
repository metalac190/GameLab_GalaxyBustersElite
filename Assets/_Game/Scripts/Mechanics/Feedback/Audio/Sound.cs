using UnityEngine;

[System.Serializable]
public class Sound
{
    // Inspector name
    [HideInInspector] public string name = "Sound";

    public AudioSource audioSource;
    public bool playOnAwake = false;
    public bool loop = false;
}