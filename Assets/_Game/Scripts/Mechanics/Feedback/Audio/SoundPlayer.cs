using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public bool playOnAwake;

    public Sound sound;


    void Awake()
    {
        if (sound == null)
        {
            Debug.LogWarning("SoundPlayer attached to " + transform.parent.name + " not given a sound to play.");
            return;
        }

        sound.audioSource = gameObject.AddComponent<AudioSource>();
        sound.audioSource.clip = sound.audioClip;
        sound.audioSource.volume = sound.volume;
        sound.audioSource.loop = sound.loop;
        if (playOnAwake)
            sound.audioSource.Play();
    }


    [ContextMenu("Test Play")]
    public void Play() => sound.audioSource.Play();


    [ContextMenu("Test Play Detach and Destroy")]
    public void PlayThenDetachAndDestroy()
    {
        if (sound.loop)
        {
            Debug.LogWarning("SoundPlayer attached to " + transform.parent.name + " trying to detach and destroy with a looping sound.");
            Play();
            return;
        }

        transform.parent = null;
        sound.audioSource.Play();
        StartCoroutine(DestroyWhenFinished());
    }

    IEnumerator DestroyWhenFinished()
    {
        while (sound.audioSource.isPlaying)
            yield return null;
        Destroy(this.gameObject);
    }
}
