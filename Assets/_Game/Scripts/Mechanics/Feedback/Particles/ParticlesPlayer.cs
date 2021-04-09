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
        for (int p = 0; p < allParticles.Length; p++)
        {
            Particles particles = allParticles[p];

            if (particles != null && particles.particleSystem)
            {
                var particlesMain = particles.particleSystem.main;
                particlesMain.playOnAwake = particles.playOnAwake;
            }

            if (particles.label.Length == 0)
                particles.label = p + ". (Write a Description Here)";
            else if (particles.label.Substring(0, 1) != p.ToString())
                particles.label = p + ". " + particles.label;
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

        if (allParticles[indexParticlesToPlay].particleSystem.main.loop)
        {
            Debug.LogWarning(name + " trying to detach, play, then destroy with a looping particle.");
            Play(indexParticlesToPlay);
            return;
        }

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

    #region Detach Play Then Destroy
    public void TryDetachPlayThenReattach(int indexParticlesToPlay)
    {
        if (!CheckIfParticlesAtIndexAreInitialized(indexParticlesToPlay)) return;

        if (allParticles[indexParticlesToPlay].particleSystem.main.loop)
        {
            Debug.LogWarning(name + " trying to detach, play, then reattach with a looping particle.");
            Play(indexParticlesToPlay);
            return;
        }

        DetachPlayThenReattach(indexParticlesToPlay);
    }

    void DetachPlayThenReattach(int indexParticlesToPlay)
    {
        Transform savedParent = transform.parent;

        transform.parent = null;
        allParticles[indexParticlesToPlay].particleSystem.Play();

        StopAllCoroutines();
        StartCoroutine(ReattachWhenFinished(indexParticlesToPlay, savedParent));
    }

    IEnumerator ReattachWhenFinished(int indexParticlesToPlay, Transform savedParent)
    {
        while (allParticles[indexParticlesToPlay].particleSystem.isPlaying)
            yield return null;

        if (savedParent != null)
            transform.parent = savedParent;
    }
    #endregion

    #region Debugging
    [ContextMenu("Test Play First Particles")]
    void TestPlayFirst() => Play(0);
    [ContextMenu("Test Detach, Play, then Destroy First Particles")]
    void TestDetachPlayThenDestroyFirst() => DetachPlayThenDestroy(0);
    [ContextMenu("Test Detach, Play, then Reattach First Particles")]
    void TestDetachPlayThenReattachFirst() => DetachPlayThenReattach(0);

    [ContextMenu("Test Play Second Particles")]
    void TestPlaySecond() => Play(1);
    [ContextMenu("Test Detach, Play, then Destroy Second Particles")]
    void TestDetachPlayThenDestroySecond() => DetachPlayThenDestroy(1);
    [ContextMenu("Test Detach, Play, then Reattach Second Particles")]
    void TestDetachPlayThenReattachSecond() => DetachPlayThenReattach(1);

    [ContextMenu("Test Play Third Particles")]
    void TestPlayThird() => Play(2);
    [ContextMenu("Test Detach, Play, then Destroy Third Particles")]
    void TestDetachPlayThenDestroyThird() => DetachPlayThenDestroy(2);
    [ContextMenu("Test Detach, Play, then Reattach Third Particles")]
    void TestDetachPlayThenReattachThird() => DetachPlayThenReattach(2);

    [ContextMenu("Test Play Fourth Particles")]
    void TestPlayFourth() => Play(3);
    [ContextMenu("Test Detach, Play, then Destroy Fourth Particles")]
    void TestDetachPlayThenDestroyFourth() => DetachPlayThenDestroy(3);
    [ContextMenu("Test Detach, Play, then Reattach Fourth Particles")]
    void TestDetachPlayThenReattachFourth() => DetachPlayThenReattach(3);

    public int GetNumParticles() { return allParticles.Length; }
    #endregion
}