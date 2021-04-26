using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] Sound[] allSounds = new Sound[1];
    bool surpressMissingSoundWarnings = true;
    const float LOOPING_FADE_IN_TIME = 1;
    const float FADE_OUT_TIME = 0.6f;

    Coroutine[] fadingInCoroutines;
    Coroutine[] fadingOutCoroutines;

    // Syncing loop and play on awake variables with Particle System
#if UNITY_EDITOR
    void OnValidate()
    {
        for (int s = 0; s < allSounds.Length; s++)
        {
            Sound sound = allSounds[s];

            if (sound != null && sound.audioSource)
            {
                sound.audioSource.loop = sound.loop;
                sound.audioSource.playOnAwake = sound.playOnAwake;

                if (!sound.useSoundVariations)
                    sound.soundVariations = null;
                else if (sound.soundVariations.Length == 0)
                {
                    sound.soundVariations = new AudioClip[1];
                    sound.soundVariations[0] = sound.audioSource.clip;
                }
                else if (sound.soundVariations.Length > 0)
                    sound.soundVariations[0] = sound.audioSource.clip;
                    
                if (!sound.usePitchRandomization)
                    sound.pitchShiftMin = sound.pitchShiftMax = 0;
            }

            if (sound.label.Length == 0)
                sound.label = s + ". (Write a Description Here)";
            if (sound.label.Substring(0, 1) != s.ToString())
                sound.label = s + ". " + sound.label;
        }
    }
#endif

    #region Setup
    void Awake()
    {
        fadingInCoroutines = new Coroutine[allSounds.Length];
        fadingOutCoroutines = new Coroutine[allSounds.Length];
        SetupAllSounds();
    }

    private void SetupAllSounds()
    {
        bool warnedAboutMissingSoundOnce = false;
        if (surpressMissingSoundWarnings)
            warnedAboutMissingSoundOnce = true;

        for (int s = 0; s < allSounds.Length; s++)
            SetupSound(ref warnedAboutMissingSoundOnce, s);
    }

    private bool SetupSound(ref bool warnedAboutMissingSoundOnce, int indexOfSound)
    {
        Sound sound = allSounds[indexOfSound];

        if (sound.audioSource != null)
        {
            // Basic properties setup
            sound.audioSource.loop = sound.loop;
            sound.audioSource.playOnAwake = sound.playOnAwake;

            sound.startVolume = sound.audioSource.volume;
            sound.audioSource.volume = sound.startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume;

            // Sound pooling setup
            sound.audioSourcePool = new List<AudioSource>();
            sound.audioSourcePool.Add(sound.audioSource);
            sound.curPoolIteration = 0;

            if (sound.loop && sound.playOnAwake) // Fade in sound if looping
                fadingInCoroutines[indexOfSound] = StartCoroutine(FadeInSound(indexOfSound));
            else if (sound.audioSource.isPlaying && !sound.playOnAwake) // Stop if not play on awake
                sound.audioSource.Stop();
            else if (!sound.audioSource.isPlaying && sound.playOnAwake) // Play if play on awake
                Play(indexOfSound);

            StartCoroutine(PauseSoundWhilePaused(indexOfSound));
        }
        else if (!warnedAboutMissingSoundOnce)
        {
            Debug.LogWarning(name + " is missing an Audio Source in at least one index");
            warnedAboutMissingSoundOnce = true;
        }

        return warnedAboutMissingSoundOnce;
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
        Sound curSound = allSounds[indexSoundToPlay];


        if (!curSound.loop && curSound.audioSourcePool.Count < Sound.MAX_POOL_SIZE && curSound.audioSourcePool[curSound.curPoolIteration].isPlaying)
            ExpandAudioSourcePool(curSound);

        if (curSound.usePitchRandomization) // Pitch randomization
            curSound.audioSourcePool[curSound.curPoolIteration].pitch = Random.Range(curSound.pitchShiftMin + 1, curSound.pitchShiftMax + 1);
        if (curSound.useSoundVariations && curSound.soundVariations.Length > 0) // Sound variations
            curSound.audioSourcePool[curSound.curPoolIteration].clip = curSound.soundVariations[Random.Range(0, curSound.soundVariations.Length)];

        if (curSound.loop) // Fade in if looping, and don't use sound pooling
        {
            if (fadingInCoroutines[indexSoundToPlay] == null)
                fadingInCoroutines[indexSoundToPlay] = StartCoroutine(FadeInSound(indexSoundToPlay));
        }
        else // Play sound normally
        {
            curSound.audioSourcePool[curSound.curPoolIteration].volume =
                curSound.startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume;

            curSound.audioSourcePool[curSound.curPoolIteration].Play();

            curSound.curPoolIteration++;
            if (curSound.curPoolIteration >= curSound.audioSourcePool.Count)
                curSound.curPoolIteration = 0;
        }
    }

    private static void ExpandAudioSourcePool(Sound curSound)
    {
        AudioSource newAudioSourceForPool =
            Instantiate(curSound.audioSource, curSound.audioSource.transform.parent);
        curSound.audioSourcePool.Add(newAudioSourceForPool);

        curSound.curPoolIteration++;
    }

    IEnumerator FadeInSound(int indexSoundToPlay)
    {
        Sound curSound = allSounds[indexSoundToPlay];

        if (fadingOutCoroutines[indexSoundToPlay] != null)
        {
            StopCoroutine(fadingOutCoroutines[indexSoundToPlay]);
            fadingOutCoroutines[indexSoundToPlay] = null;
        }

        curSound.audioSourcePool[curSound.curPoolIteration].volume = 0;
        curSound.audioSourcePool[curSound.curPoolIteration].Play();
        while (curSound.audioSourcePool[curSound.curPoolIteration].volume <
            curSound.startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume)
        {
            if (fadingOutCoroutines[indexSoundToPlay] != null)
            {
                StopCoroutine(fadingOutCoroutines[indexSoundToPlay]);
                fadingOutCoroutines[indexSoundToPlay] = null;
            }
            curSound.audioSourcePool[curSound.curPoolIteration].volume =
                Mathf.Clamp(curSound.audioSourcePool[curSound.curPoolIteration].volume +
                curSound.startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume
                * 0.02f / LOOPING_FADE_IN_TIME, 0, 1);
            yield return new WaitForSeconds(0.02f);
        }
        curSound.audioSourcePool[curSound.curPoolIteration].volume =
            curSound.startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume;

        fadingInCoroutines[indexSoundToPlay] = null;
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
        Sound curSound = allSounds[indexSoundToStop];

        curSound.curPoolIteration--;
        if (curSound.curPoolIteration < 0)
            curSound.curPoolIteration = curSound.audioSourcePool.Count - 1;

        if (fadingOutCoroutines[indexSoundToStop] == null)
            fadingOutCoroutines[indexSoundToStop] = StartCoroutine(FadeOutSound(indexSoundToStop));
    }

    IEnumerator FadeOutSound(int indexSoundToPlay)
    {
        Sound curSound = allSounds[indexSoundToPlay];

        if (fadingInCoroutines[indexSoundToPlay] != null)
        {
            StopCoroutine(fadingInCoroutines[indexSoundToPlay]);
            fadingInCoroutines[indexSoundToPlay] = null;
        }

        while (curSound.audioSourcePool[curSound.curPoolIteration].volume > 0.01)
        {
            if (fadingInCoroutines[indexSoundToPlay] != null)
            {
                StopCoroutine(fadingInCoroutines[indexSoundToPlay]);
                fadingInCoroutines[indexSoundToPlay] = null;
            }
            curSound.audioSourcePool[curSound.curPoolIteration].volume =
                Mathf.Clamp(curSound.audioSourcePool[curSound.curPoolIteration].volume - 0.02f / FADE_OUT_TIME, 0, 1);
            yield return new WaitForSeconds(0.02f);
        }
        curSound.audioSourcePool[curSound.curPoolIteration].volume = 0;
        curSound.audioSourcePool[curSound.curPoolIteration].Stop();

        fadingOutCoroutines[indexSoundToPlay] = null;
    }
    #endregion

    #region Detach Play Then Destroy
    public void TryDetachPlayThenDestroy(int indexSoundToPlay)
    {
        if (!CheckIfSoundAtIndexIsInitialized(indexSoundToPlay)) return;
        if (transform.parent == null) return;

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

        allSounds[indexSoundToPlay].audioSourcePool[allSounds[indexSoundToPlay].curPoolIteration].volume =
            allSounds[indexSoundToPlay].startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume;
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

    #region Detach Play Then Reattach
    public void TryDetachPlayThenReattach(int indexSoundToPlay)
    {
        if (!CheckIfSoundAtIndexIsInitialized(indexSoundToPlay)) return;
        if (transform.parent == null) return;

        if (allSounds[indexSoundToPlay].loop)
        {
            Debug.LogWarning(name + " trying to detach, play, then reattach with a looping sound.");
            Play(indexSoundToPlay);
            return;
        }

        DetachPlayThenReattach(indexSoundToPlay);
    }

    void DetachPlayThenReattach(int indexSoundToPlay)
    {
        Transform savedParent = transform.parent;

        transform.parent = null;

        allSounds[indexSoundToPlay].audioSourcePool[allSounds[indexSoundToPlay].curPoolIteration].volume =
            allSounds[indexSoundToPlay].startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume;
        allSounds[indexSoundToPlay].audioSource.Play();

        StopAllCoroutines();
        StartCoroutine(ReattachWhenFinished(indexSoundToPlay, savedParent));
    }

    IEnumerator ReattachWhenFinished(int indexSoundToPlay, Transform savedParent)
    {
        while (allSounds[indexSoundToPlay].audioSource.isPlaying)
            yield return null;

        if (savedParent != null)
            transform.parent = savedParent;
    }
    #endregion

    #region Pause Sound While Paused
    IEnumerator PauseSoundWhilePaused(int indexOfSound)
    {
        // If looping
        if (allSounds[indexOfSound].loop)
        {
            while (true)
            {
                if (GameManager.gm && GameManager.gm.Paused && allSounds[indexOfSound].audioSource.isPlaying)
                {
                    allSounds[indexOfSound].audioSource.Pause();
                    yield return new WaitForSeconds(0.01f);
                    allSounds[indexOfSound].audioSource.UnPause();
                }
                yield return new WaitForSecondsRealtime(0.06f);
            }
        }
        // If not looping
        else
        {
            bool soundPlaying;
            while (true)
            {
                if (GameManager.gm && GameManager.gm.Paused)
                {
                    soundPlaying = false;
                    foreach (AudioSource audioSource in allSounds[indexOfSound].audioSourcePool)
                        if (audioSource.isPlaying)
                            soundPlaying = true;

                    if (soundPlaying)
                    {
                        for (int s = 0; s < allSounds[indexOfSound].audioSourcePool.Count; s++)
                            allSounds[indexOfSound].audioSourcePool[s].Pause();

                        yield return new WaitForSeconds(0.01f);

                        for (int s = 0; s < allSounds[indexOfSound].audioSourcePool.Count; s++)
                            allSounds[indexOfSound].audioSourcePool[s].UnPause();
                    }
                }
                yield return new WaitForSecondsRealtime(0.06f);
            }
        }
    }
    #endregion

    #region Volume Sliders
    public void RefreshSoundVolumes()
    {
        for (int s = 0; s < allSounds.Length; s++)
        {
            if (fadingInCoroutines[s] == null && fadingOutCoroutines[s] == null)
                allSounds[s].audioSource.volume =
                    allSounds[s].startVolume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.soundVolume;
        }
    }
    #endregion

    #region Debugging
    [ContextMenu("Test Play First Sound")]
    void TestPlayFirst() => TryPlay(0);
    [ContextMenu("Test Detach, Play, then Destroy First Sound")]
    void TestDetachPlayThenDestroyFirst() => TryDetachPlayThenDestroy(0);
    [ContextMenu("Test Detach, Play, then Reattach First Sound")]
    void TestDetachPlayThenReattachFirst() => TryDetachPlayThenReattach(0);

    [ContextMenu("Test Play Second Sound")]
    void TestPlaySecond() => TryPlay(1);
    [ContextMenu("Test Detach, Play, then Destroy Second Sound")]
    void TestDetachPlayThenDestroySecond() => TryDetachPlayThenDestroy(1);
    [ContextMenu("Test Detach, Play, then Reattach Second Sound")]
    void TestDetachPlayThenReattachSecond() => TryDetachPlayThenReattach(1);

    [ContextMenu("Test Play Third Sound")]
    void TestPlayThird() => TryPlay(2);
    [ContextMenu("Test Detach, Play, then Destroy Third Sound")]
    void TestDetachPlayThenDestroyThird() => TryDetachPlayThenDestroy(2);
    [ContextMenu("Test Detach, Play, then Reattach Third Sound")]
    void TestDetachPlayThenReattachThird() => TryDetachPlayThenReattach(2);

    [ContextMenu("Test Play Fourth Sound")]
    void TestPlayFourth() => TryPlay(3);
    [ContextMenu("Test Detach, Play, then Destroy Fourth Sound")]
    void TestDetachPlayThenDestroyFourth() => TryDetachPlayThenDestroy(3);
    [ContextMenu("Test Detach, Play, then Reattach Fourth Sound")]
    void TestDetachPlayThenReattachFourth() => TryDetachPlayThenReattach(3);

    [ContextMenu("Test Sound Pooling First Sound x50")]
    void TestSoundPoolingFive() => TestSoundPoolingHelper(1, 5);
    [ContextMenu("Test Sound Pooling First Sound x10")]
    void TestSoundPoolingTen() => TestSoundPoolingHelper(1, 10);
    [ContextMenu("Test Sound Pooling First Sound x30")]
    void TestSoundPoolingThirty() => TestSoundPoolingHelper(1, 30);
    public void TestSoundPoolingHelper(int index, int numTest) => StartCoroutine(TestSoundPooling(index, numTest));
    IEnumerator TestSoundPooling(int index, int numTest)
    {
        for (int x = 0; x < numTest; x++)
        {
            Play(index);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public int GetNumSounds() { return allSounds.Length; }
    public string GetSoundLabel(int indexOfSound) { return allSounds[indexOfSound].label; }
    public float GetSoundStartVolume(int indexOfSound) { return allSounds[indexOfSound].startVolume; }
    #endregion
}