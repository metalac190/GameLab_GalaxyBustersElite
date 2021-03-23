using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] Sound[] allSounds = new Sound[1];
    bool surpressMissingSoundWarnings = true;

    // Syncing loop and play on awake variables with Particle System
#if UNITY_EDITOR
    void OnValidate()
    {
        foreach (Sound sound in allSounds)
        {
            if (sound != null && sound.audioSource)
            {
                sound.audioSource.loop = sound.loop;
                sound.audioSource.playOnAwake = sound.playOnAwake;
            }
        }
    }
#endif

    #region Setup
    void Awake()
    {
        bool warnedAboutMissingSoundOnce = false;
        if (surpressMissingSoundWarnings)
            warnedAboutMissingSoundOnce = true;

        foreach (Sound sound in allSounds)
        {
            if (sound.audioSource != null)
            {
                sound.audioSource.loop = sound.loop;
                sound.audioSource.playOnAwake = sound.playOnAwake;
                if (sound.audioSource.isPlaying && !sound.playOnAwake)
                    sound.audioSource.Stop();
            }
            else if (!warnedAboutMissingSoundOnce)
            {
                Debug.LogWarning(name + " is missing an Audio Source in at least one index");
                warnedAboutMissingSoundOnce = true;
            }
        }
    }

    void Start()
    {
        CheckIfHasParent();

        CheckIfHasAnySounds();
    }

    #endregion

    #region Initialization Checks
    void CheckIfHasParent()
    {
        if (transform.parent == null)
            Debug.LogWarning(name + " has no parent.");
    }

    void CheckIfHasAnySounds()
    {
        if (allSounds.Length == 0)
            Debug.LogWarning(name + " not given any sounds to play.");
    }

    bool CheckIfSoundAtIndexIsInitialized(int index)
    {
        if (allSounds.Length == 0 || index >= allSounds.Length || allSounds[index].audioSource == null)
        {
            if (!surpressMissingSoundWarnings)
                Debug.LogWarning(name + " tried playing/stopping a nonexistent sound at index " + index + ".");
            return false;
        }
        else
            return true;
    }
    #endregion


    #region Play
    public void TryPlay(int indexSoundToPlay)
    {
        if (!CheckIfSoundAtIndexIsInitialized(indexSoundToPlay)) return;

        Play(indexSoundToPlay);
    }

    void Play(int indexSoundToPlay) { allSounds[indexSoundToPlay].audioSource.Play(); }
    #endregion

    #region Stop
    public void TryStop(int indexSoundToPlay)
    {
        if (!CheckIfSoundAtIndexIsInitialized(indexSoundToPlay)) return;

        Stop(indexSoundToPlay);
    }

    void Stop(int indexSoundToPlay) { allSounds[indexSoundToPlay].audioSource.Stop(); }
    #endregion

    #region Detach Play Then Destroy
    public void TryDetachPlayThenDestroy(int indexSoundToPlay)
    {
        if (!CheckIfSoundAtIndexIsInitialized(indexSoundToPlay)) return;

        if (allSounds[indexSoundToPlay].loop)
        {
            Debug.LogWarning(name + " trying to detach and destroy with a looping sound.");
            Play(indexSoundToPlay);
            return;
        }

        DetachPlayThenDestroy(indexSoundToPlay);
    }

    void DetachPlayThenDestroy(int indexSoundToPlay)
    {
        transform.parent = null;
        allSounds[indexSoundToPlay].audioSource.Play();

        StopAllCoroutines();
        StartCoroutine(DestroyWhenFinished(indexSoundToPlay));
    }

    IEnumerator DestroyWhenFinished(int indexSoundToPlay)
    {
        while (allSounds[indexSoundToPlay].audioSource.isPlaying)
            yield return null;

        Destroy(this.gameObject);
    }
    #endregion


    #region Debugging
    [ContextMenu("Test Play First Sound")]
    void TestPlayFirst() => TryPlay(0);
    [ContextMenu("Test Detach, Play, then Destroy First Sound")]
    void TestPlayThenDetachAndDestroyFirst() => TryDetachPlayThenDestroy(0);

    [ContextMenu("Test Play Second Sound")]
    void TestPlaySecond() => TryPlay(1);
    [ContextMenu("Test Detach, Play, then Destroy Second Sound")]
    void TestPlayThenDetachAndDestroySecond() => TryDetachPlayThenDestroy(1);

    [ContextMenu("Test Play Third Sound")]
    void TestPlayThird() => TryPlay(2);
    [ContextMenu("Test Detach, Play, then Destroy Third Sound")]
    void TestPlayThenDetachAndDestroyThird() => TryDetachPlayThenDestroy(2);

    [ContextMenu("Test Play Fourth Sound")]
    void TestPlayFourth() => TryPlay(3);
    [ContextMenu("Test Detach, Play, then Destroy Fourth Sound")]
    void TestPlayThenDetachAndDestroyFourth() => TryDetachPlayThenDestroy(3);
    #endregion
}