using System.Collections;
using UnityEngine;

public class ParticlesPlayer : MonoBehaviour
{
    [SerializeField] Particles[] allParticles = new Particles[1];


    private void Awake()
    {
        foreach (Particles particles in allParticles)
            if (particles.playOnAwake)
                particles.particleSystem.Play();
    }

    private void Start()
    {
        if (allParticles.Length == 0)
            Debug.LogWarning("ParticlesPlayer attached to " + transform.parent.name + " not given any particles to play.");
    }



    public void Play(int indexParticlesToPlay)
    {
        if (allParticles.Length == 0 || indexParticlesToPlay >= allParticles.Length)
        {
            Debug.LogWarning("ParticlesPlayer attached to " + transform.parent.name + " tried playing nonexistent particles at index " + indexParticlesToPlay + ".");
            return;
        }

        allParticles[indexParticlesToPlay].particleSystem.Play();
    }

    public void DetachPlayThenDestroy(int indexParticlesToPlay)
    {
        if (allParticles.Length == 0 || indexParticlesToPlay >= allParticles.Length)
        {
            Debug.LogWarning("ParticlesPlayer attached to " + transform.parent.name + " tried playing nonexistent particles at index " + indexParticlesToPlay + ".");
            return;
        }

        transform.parent = null;
        allParticles[indexParticlesToPlay].particleSystem.Play();
        StartCoroutine(DestroyWhenFinished(indexParticlesToPlay));
    }
    IEnumerator DestroyWhenFinished(int indexParticlesToPlay)
    {
        while (allParticles[indexParticlesToPlay].particleSystem.isPlaying)
            yield return null;
        Destroy(this.gameObject);
    }



    #region Debug
    [ContextMenu("Test Play First Particles")]
    void TestPlayFirst() => Play(0);
    [ContextMenu("Test Detach, Play, then Destroy First Particles")]
    void TestPlayThenDetachAndDestroyFirst() => DetachPlayThenDestroy(0);

    [ContextMenu("Test Play Second Particles")]
    void TestPlaySecond() => Play(1);
    [ContextMenu("Test Detach, Play, then Destroy Second Particles")]
    void TestPlayThenDetachAndDestroySecond() => DetachPlayThenDestroy(1);

    [ContextMenu("Test Play Third Particles")]
    void TestPlayThird() => Play(2);
    [ContextMenu("Test Detach, Play, then Destroy Third Particles")]
    void TestPlayThenDetachAndDestroyThird() => DetachPlayThenDestroy(2);

    [ContextMenu("Test Play Fourth Particles")]
    void TestPlayFourth() => Play(3);
    [ContextMenu("Test Detach, Play, then Destroy Fourth Particles")]
    void TestPlayThenDetachAndDestroyFourth() => DetachPlayThenDestroy(3);
    #endregion
}