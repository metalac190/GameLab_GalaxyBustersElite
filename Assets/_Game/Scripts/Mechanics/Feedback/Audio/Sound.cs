using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    // Inspector name
    public string label = "Sound";

    [Space(5)]
    public AudioSource audioSource;

    [Space(5)]
    public bool playOnAwake = false;
    public bool loop = false;

    [Header("Sound Variations")]
    public bool useSoundVariations;
    public AudioClip[] soundVariations;

    [Header("Pitch Randomization")]
    public bool usePitchRandomization;
    [Range(-1, 0)]
    public float pitchShiftMin = 0;
    [Range(0, 1)]
    public float pitchShiftMax = 0;

    [Header("Sound Pooling")]
    public const int MAX_POOL_SIZE = 15;
    [HideInInspector] public int curPoolIteration = 0;
    [HideInInspector] public List<AudioSource> audioSourcePool;
}