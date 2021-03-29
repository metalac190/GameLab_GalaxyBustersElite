using UnityEngine;

[System.Serializable]
public class Particles
{
    // Inspector name
    [HideInInspector] public string name = "Particles";
    public string label = "(Write a Description Here)";

    [Space(5)]
    public ParticleSystem particleSystem;
    public bool playOnAwake;
}
