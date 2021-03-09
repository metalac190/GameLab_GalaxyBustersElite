using UnityEngine;

[System.Serializable]
public class MusicTrack
{
    [HideInInspector] public AudioSource audioSource;

    public AudioClip musicTrack;
    [Range(0, 1)] public float volume = 1;
}
