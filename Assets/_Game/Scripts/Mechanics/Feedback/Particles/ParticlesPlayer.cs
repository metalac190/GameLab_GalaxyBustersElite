using System.Collections;
using UnityEngine;

public class ParticlesPlayer : MonoBehaviour
{
    [SerializeField] Particles[] allParticles = new Particles[1];


#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (Particles particles in allParticles)
        {
            if (particles.particleSystem != null)
            {
                var particlesMain = particles.particleSystem.main;
                particlesMain.playOnAwake = particles.playOnAwake;
            }
        }
    }
#endif


    private void Awake()
    {
        foreach (Particles particles in allParticles)
        {
            if (particles.particleSystem != null)
            {
                if (particles.particleSystem.isPlaying && !particles.playOnAwake)
                    particles.particleSystem.Stop();
            }
            else
                Debug.LogWarning("ParticlesPlayer " + name + " is missing a Particle System in at least one index");
        }
    }

    private void Start()
    {
        if (transform.parent == null)
            Debug.LogWarning("ParticlesPlayer " + name + " has no parent.");

        if (allParticles.Length == 0)
            Debug.LogWarning("ParticlesPlayer " + name + " not given any sounds to play.");
    }


    #region Play
    public void TryPlay(int indexParticlesToPlay)
    {
        if (allParticles.Length == 0 || indexParticlesToPlay >= allParticles.Length)
        {
            Debug.LogWarning("ParticlesPlayer " + name + " tried playing nonexistent particles at index " + indexParticlesToPlay + ".");
            return;
        }

        Play(indexParticlesToPlay);
    }

    private void Play(int indexParticlesToPlay) { allParticles[indexParticlesToPlay].particleSystem.Play(); }
    #endregion


    #region DetachPlayThenDestroy
    public void TryDetachPlayThenDestroy(int indexParticlesToPlay)
    {
        if (allParticles.Length == 0 || indexParticlesToPlay >= allParticles.Length)
        {
            Debug.LogWarning("ParticlesPlayer " + name + " tried playing nonexistent particles at index " + indexParticlesToPlay + ".");
            return;
        }

        DetachPlayThenDestroy(indexParticlesToPlay);
    }

    private void DetachPlayThenDestroy(int indexParticlesToPlay)
    {
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
    #endregion


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