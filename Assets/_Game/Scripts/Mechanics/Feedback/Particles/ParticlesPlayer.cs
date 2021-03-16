using System.Collections;
using UnityEngine;

public class ParticlesPlayer : MonoBehaviour
{
    [SerializeField] Particles[] allParticles = new Particles[1];
    bool surpressMissingParticlesWarnings = true;

    // Syncing play on awake variables with Particle System
#if UNITY_EDITOR
    void OnValidate()
    {
        foreach (Particles particles in allParticles)
        {
            if (particles != null && particles.particleSystem)
            {
                var particlesMain = particles.particleSystem.main;
                particlesMain.playOnAwake = particles.playOnAwake;
            }
        }
    }
#endif

    #region Setup
    void Awake()
    {
        bool warnedAboutMissingParticlesOnce = false;
        if (surpressMissingParticlesWarnings)
            warnedAboutMissingParticlesOnce = true;

        foreach (Particles particles in allParticles)
        {
            if (particles.particleSystem != null)
            {
                if (particles.particleSystem.isPlaying && !particles.playOnAwake)
                    particles.particleSystem.Stop();
            }
            else if (!warnedAboutMissingParticlesOnce)
            {
                Debug.LogWarning(name + " is missing a Particle System in at least one index");
                warnedAboutMissingParticlesOnce = true;
            }
        }
    }

    void Start()
    {
        CheckIfHasParent();

        CheckIfHasAnyParticles();
    }
    #endregion

    #region Initialization Checks
    void CheckIfHasParent()
    {
        if (transform.parent == null)
            Debug.LogWarning(name + " has no parent.");
    }

    void CheckIfHasAnyParticles()
    {
        if (allParticles.Length == 0)
            Debug.LogWarning(name + " not given any particles to play.");
    }

    bool CheckIfParticlesAtIndexAreInitialized(int index)
    {
        if (allParticles.Length == 0 || index >= allParticles.Length || allParticles[index].particleSystem == null)
        {
            if (!surpressMissingParticlesWarnings)
                Debug.LogWarning(name + " tried playing nonexistent particles at index " + index + ".");
            return false;
        }
        else
            return true;
    }
    #endregion


    #region Play
    public void TryPlay(int indexParticlesToPlay)
    {
        if (!CheckIfParticlesAtIndexAreInitialized(indexParticlesToPlay)) return;

        Play(indexParticlesToPlay);
    }

    void Play(int indexParticlesToPlay) { allParticles[indexParticlesToPlay].particleSystem.Play(); }
    #endregion

    #region Detach Play Then Destroy
    public void TryDetachPlayThenDestroy(int indexParticlesToPlay)
    {
        if (!CheckIfParticlesAtIndexAreInitialized(indexParticlesToPlay)) return;

        DetachPlayThenDestroy(indexParticlesToPlay);
    }

    void DetachPlayThenDestroy(int indexParticlesToPlay)
    {
        transform.parent = null;
        allParticles[indexParticlesToPlay].particleSystem.Play();

        StopAllCoroutines();
        StartCoroutine(DestroyWhenFinished(indexParticlesToPlay));
    }

    IEnumerator DestroyWhenFinished(int indexParticlesToPlay)
    {
        while (allParticles[indexParticlesToPlay].particleSystem.isPlaying)
            yield return null;

        Destroy(this.gameObject);
    }
    #endregion


    #region Debugging
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