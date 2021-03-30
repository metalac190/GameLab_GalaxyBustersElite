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

        for (int s = 0; s < allSounds.Length; s++)
        {
            Sound sound = allSounds[s];

            if (sound.audioSource != null)
            {
                sound.audioSource.loop = sound.loop;
                sound.audioSource.playOnAwake = sound.playOnAwake;

                // If using sound pooling
                if (sound.soundPoolSize > 1)
                {
                    sound.audioSourcePool = new AudioSource[sound.soundPoolSize];
                    sound.audioSourcePool[0] = sound.audioSource;
                    for (int i = 1; i < sound.soundPoolSize; i++)
                    {
                        //sound.audioSourcePool[i] = Instantiate(sound.audioSource.gameObject, transform).GetComponent<AudioSource>();
                        sound.audioSourcePool[i] = Instantiate(sound.audioSource, transform);
                        if (sound.audioSourcePool[i].isPlaying)
                            sound.audioSourcePool[i].Stop();
                    }
                }

                if (!sound.audioSource.isPlaying && sound.playOnAwake)
                    Play(s);
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

    void Play(int indexSoundToPlay)
    {
        if (allSounds[indexSoundToPlay].soundPoolSize == 1) // Not sound pooling
            allSounds[indexSoundToPlay].audioSource.Play();
        else // Sound pooling
        {
            print("Playing sound #" + allSounds[indexSoundToPlay].curPoolIteration);
            allSounds[indexSoundToPlay].audioSourcePool[allSounds[indexSoundToPlay].curPoolIteration].Play();

            allSounds[indexSoundToPlay].curPoolIteration++;
            if (allSounds[indexSoundToPlay].curPoolIteration >= allSounds[indexSoundToPlay].soundPoolSize)
                allSounds[indexSoundToPlay].curPoolIteration = 0;
        }
    }
    #endregion

    #region Stop
    public void TryStop(int indexSoundToStop)
    {
        if (!CheckIfSoundAtIndexIsInitialized(indexSoundToStop)) return;

        Stop(indexSoundToStop);
    }

    void Stop(int indexSoundToStop)
    {
        if (allSounds[indexSoundToStop].soundPoolSize == 1) // Not sound pooling
            allSounds[indexSoundToStop].audioSource.Stop();
        else // Sound pooling
        {
            int curPoolLastIteration = allSounds[indexSoundToStop].curPoolIteration - 1;
            if (indexSoundToStop < 0)
                curPoolLastIteration = allSounds[indexSoundToStop].soundPoolSize - 1;

            allSounds[indexSoundToStop].audioSourcePool[curPoolLastIteration].Stop();
        }
    }
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

    [ContextMenu("Test Sound Pooling First Sound")]
    void TestSoundPoolingFirst() => StartCoroutine(SoundPoolingTest(0));
    IEnumerator SoundPoolingTest(int indexSoundToTestPooling)
    {
        for (int x = 0; x < 10; x++)
        {
            Play(indexSoundToTestPooling);
            yield return new WaitForSeconds(0.15f);
        }
    }
    #endregion
}