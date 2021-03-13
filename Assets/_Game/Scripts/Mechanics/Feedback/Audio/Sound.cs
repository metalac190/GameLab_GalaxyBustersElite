using UnityEngine;

[System.Serializable]
public class Sound
{
    // Inspector name
    [HideInInspector] public string name = "Sound";
    public string label = "(Write a Description Here)";

    [Space(5)]
    public AudioSource audioSource;
    public bool playOnAwake = false;
    public bool loop = false;
}