﻿using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] Sound[] allSounds = new Sound[1];


    void Awake()
    {
        if (allSounds.Length == 0)
        {
            Debug.LogWarning("SoundPlayer attached to " + transform.parent.name + " not given any sounds to play.");
            return;
        }

        foreach (Sound sound in allSounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
            if (sound.playOnAwake)
                sound.audioSource.Play();
        }
    }



    public void Play(int indexSoundToPlay)
    {
        if (allSounds.Length == 0 || indexSoundToPlay >= allSounds.Length)
        {
            Debug.LogWarning("SoundPlayer attached to " + transform.parent.name + " tried playing nonexistent sound at index " + indexSoundToPlay + ".");
            return;
        }

        allSounds[indexSoundToPlay].audioSource.Play();
    }

    public void PlayThenDetachAndDestroy(int indexSoundToPlay)
    {
        if (allSounds.Length == 0 || indexSoundToPlay >= allSounds.Length)
        {
            Debug.LogWarning("SoundPlayer attached to " + transform.parent.name + " tried playing nonexistent sound at index " + indexSoundToPlay + ".");
            return;
        }

        if (allSounds[indexSoundToPlay].loop)
        {
            Debug.LogWarning("SoundPlayer attached to " + transform.parent.name + " trying to detach and destroy with a looping sound.");
            Play(indexSoundToPlay);
            return;
        }

        transform.parent = null;
        allSounds[indexSoundToPlay].audioSource.Play();
        StartCoroutine(DestroyWhenFinished(indexSoundToPlay));
    }
    IEnumerator DestroyWhenFinished(int indexSoundToPlay)
    {
        while (allSounds[indexSoundToPlay].audioSource.isPlaying)
            yield return null;
        Destroy(this.gameObject);
    }



    #region Debug
    [ContextMenu("Test Play First Sound")]
    void TestPlayFirst() => Play(0);
    [ContextMenu("Test Play Then Detach and Destroy First Sound")]
    void TestPlayThenDetachAndDestroyFirst() => PlayThenDetachAndDestroy(0);

    [ContextMenu("Test Play Second Sound")]
    void TestPlaySecond() => Play(1);
    [ContextMenu("Test Play Then Detach and Destroy Second Sound")]
    void TestPlayThenDetachAndDestroySecond() => PlayThenDetachAndDestroy(1);

    [ContextMenu("Test Play Third Sound")]
    void TestPlayThird() => Play(2);
    [ContextMenu("Test Play Then Detach and Destroy Third Sound")]
    void TestPlayThenDetachAndDestroyThird() => PlayThenDetachAndDestroy(2);

    [ContextMenu("Test Play Fourth Sound")]
    void TestPlayFourth() => Play(3);
    [ContextMenu("Test Play Then Detach and Destroy Fourth Sound")]
    void TestPlayThenDetachAndDestroyFourth() => PlayThenDetachAndDestroy(3);
    #endregion
}