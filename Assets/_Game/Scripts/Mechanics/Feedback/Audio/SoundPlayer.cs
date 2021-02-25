using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] Sound[] allSounds = new Sound[1];


#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (Sound sound in allSounds)
        {
            if (sound.audioSource != null)
            {
                sound.audioSource.loop = sound.loop;
                sound.audioSource.playOnAwake = sound.playOnAwake;
            }
        }
    }
#endif


    void Awake()
    {
        foreach (Sound sound in allSounds)
        {
            if (sound.audioSource != null)
            {
                sound.audioSource.loop = sound.loop;
                sound.audioSource.playOnAwake = sound.playOnAwake;
                if (sound.audioSource.isPlaying && !sound.playOnAwake)
                    sound.audioSource.Stop();
            }
            else
                Debug.LogWarning("SoundPlayer " + name + " is missing an Audio Source in at least one index");
        }
    }

    private void Start()
    {
        if (transform.parent == null)
            Debug.LogWarning("SoundPlayer " + name + " has no parent.");

        if (allSounds.Length == 0)
            Debug.LogWarning("SoundPlayer " + name + " not given any sounds to play.");
    }


    #region Play
    public void TryPlay(int indexSoundToPlay)
    {
        if (allSounds.Length == 0 || indexSoundToPlay >= allSounds.Length || allSounds[indexSoundToPlay].audioSource == null)
        {
            Debug.LogWarning("SoundPlayer " + name + " tried playing nonexistent sound at index " + indexSoundToPlay + ".");
            return;
        }

        Play(indexSoundToPlay);
    }

    private void Play(int indexSoundToPlay) { allSounds[indexSoundToPlay].audioSource.Play(); }
    #endregion


    #region DetachPlayThenDestroy
    public void TryDetachPlayThenDestroy(int indexSoundToPlay)
    {
        if (allSounds.Length == 0 || indexSoundToPlay >= allSounds.Length || allSounds[indexSoundToPlay].audioSource == null)
        {
            Debug.LogWarning("SoundPlayer " + name + " tried playing nonexistent sound at index " + indexSoundToPlay + ".");
            return;
        }

        if (allSounds[indexSoundToPlay].loop)
        {
            Debug.LogWarning("SoundPlayer " + name + " trying to detach and destroy with a looping sound.");
            Play(indexSoundToPlay);
            return;
        }

        DetachPlayThenDestroy(indexSoundToPlay);
    }

    private void DetachPlayThenDestroy(int indexSoundToPlay)
    {
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
    #endregion


    #region Debug
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