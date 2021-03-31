using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    // Inspector name
    [HideInInspector] public string name = "Sound";
    public string label = "(Write a Description Here)";

    public AudioSource audioSource;

    [Header("Sound Properties")]
    public bool playOnAwake = false;
    public bool loop = false;

    [Header("Sound Pooling")]
    //[Range(1, 10)]
    [HideInInspector] public const int MAX_POOL_SIZE = 15;
    [HideInInspector] public int curPoolIteration = 0;
    [HideInInspector] public List<AudioSource> audioSourcePool;
}