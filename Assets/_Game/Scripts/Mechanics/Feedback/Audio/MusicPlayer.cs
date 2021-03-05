using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    [Header("Standard Track")]
    [SerializeField] MusicTrack music;
    [SerializeField] bool fadeIn;
    [Range(0.01f, 20)]
    [SerializeField] float fadeInDuration = 0.8f;
    const float FADE_OUT_DURATION = 2;

    [Header("Alternative Track")]
    [SerializeField] MusicTrack altMusic;
    bool altMusicPlaying = false;
    [Range(0.01f, 20)]
    [SerializeField] float crossFadeDuration = 0.8f;

    #region Setup
    private void Awake()
    {
        SingletonChecking();

        if (CheckStandardMusicTrack())
            MusicTracksSetup();
    }

    private void SingletonChecking()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void MusicTracksSetup()
    {
        if (CheckStandardMusicTrack())
            StandardMusicTrackSetup();

        if (altMusic.musicTrack != null)
            AlternativeMusicTrackSetup();
    }

    private void StandardMusicTrackSetup()
    {
        music.audioSource = gameObject.AddComponent<AudioSource>();
        music.audioSource.clip = music.musicTrack;
        music.audioSource.loop = true;
        if (fadeIn)
            FadeIn();
        else
        {
            music.audioSource.volume = music.volume;
            music.audioSource.Play();
        }
    }

    private void AlternativeMusicTrackSetup()
    {
        altMusic.audioSource = gameObject.AddComponent<AudioSource>();
        altMusic.audioSource.clip = altMusic.musicTrack;
        altMusic.audioSource.loop = true;
    }
    #endregion


    #region Initialization Checks
    private bool CheckStandardMusicTrack()
    {
        if (music.musicTrack != null)
            return true;
        else
        {
            Debug.LogWarning("MusicPlayer " + name + " is missing a standard music track");
            return false;
        }
    }

    private bool CheckAlternativeMusicTrack()
    {
        if (altMusic.musicTrack != null)
            return true;
        else
        {
            Debug.LogWarning("MusicPlayer " + name + " is missing an alternative music track");
            return false;
        }
    }
    #endregion


    #region Fade In
    void FadeIn()
    {
        if (!CheckStandardMusicTrack()) return;

        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine());
    }
    IEnumerator FadeInCoroutine()
    {
        music.audioSource.volume = 0;
        music.audioSource.Play();
        while (music.audioSource.volume < music.volume)
        {
            yield return new WaitForFixedUpdate();
            music.audioSource.volume += Time.fixedDeltaTime / fadeInDuration * music.volume;
        }
        music.audioSource.volume = music.volume;
    }
    #endregion


    #region Fade Out
    public void FadeOut()
    {
        if (!altMusicPlaying && !CheckStandardMusicTrack()) return;
        else if (altMusicPlaying && !CheckAlternativeMusicTrack()) return;

        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(altMusicPlaying));

        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    IEnumerator FadeOutCoroutine(bool alternativeTrack = false)
    {
        MusicTrack fadingOutMusicTrack = (alternativeTrack) ? altMusic : music;

        float startingVolume = fadingOutMusicTrack.audioSource.volume = fadingOutMusicTrack.volume;
        while (fadingOutMusicTrack.audioSource.volume > 0)
        {
            yield return new WaitForFixedUpdate();
            fadingOutMusicTrack.audioSource.volume -= Time.fixedDeltaTime / FADE_OUT_DURATION * startingVolume;
        }
        fadingOutMusicTrack.audioSource.Stop();
    }
    #endregion

    #region Cross Fade
    [ContextMenu("Cross Fade Tracks")]
    public void CrossFadeBetweenTracks()
    {
        if (!altMusicPlaying)
        {
            if (!CheckAlternativeMusicTrack()) return;

            StopAllCoroutines();
            StartCoroutine(CrossFadeCoroutine(true));
            altMusicPlaying = true;
        }
        else
        {
            if (!CheckStandardMusicTrack()) return;

            StopAllCoroutines();
            StartCoroutine(CrossFadeCoroutine(false));
            altMusicPlaying = false;
        }
    }
    IEnumerator CrossFadeCoroutine(bool standardToAlternative)
    {
        MusicTrack startMusicTrack = (standardToAlternative) ? music : altMusic;
        MusicTrack endMusicTrack = (standardToAlternative) ? altMusic : music;

        float startingVolume = startMusicTrack.audioSource.volume = startMusicTrack.volume;
        endMusicTrack.audioSource.volume = 0;
        endMusicTrack.audioSource.Play();
        while (startMusicTrack.audioSource.volume > 0 || endMusicTrack.audioSource.volume < endMusicTrack.volume)
        {
            yield return new WaitForFixedUpdate();
            startMusicTrack.audioSource.volume -= Time.fixedDeltaTime / crossFadeDuration * startingVolume;
            endMusicTrack.audioSource.volume += Time.fixedDeltaTime / crossFadeDuration * endMusicTrack.volume;
        }
        endMusicTrack.audioSource.volume = endMusicTrack.volume;
        startMusicTrack.audioSource.Stop();
    }
    #endregion
}