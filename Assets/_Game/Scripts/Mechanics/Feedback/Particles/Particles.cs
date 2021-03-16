using UnityEngine;

[System.Serializable]
public class Particles
{
    // Inspector name
    [HideInInspector] public string name = "Particles";

    public ParticleSystem particleSystem;
    public bool playOnAwake;
}
