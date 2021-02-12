using UnityEngine.Audio;
using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioClip audioClip;

    [Range(0, 1)]
    public float volume = 0.5f;
    public bool loop = false;

    [HideInInspector]
    public AudioSource audioSource;
}
