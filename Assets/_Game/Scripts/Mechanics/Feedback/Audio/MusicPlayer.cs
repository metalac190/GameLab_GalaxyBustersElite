using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    [Header("Standard Track")]
    [SerializeField] MusicTrack standardMusic;
    [SerializeField] bool fadeIn;
    [Range(0.01f, 20)]
    [SerializeField] float fadeInDuration = 0.8f;
    bool fadingOut; // Used to make sure MusicPlayer doesn't crossfade when in the middle of fading out
    const float FADE_OUT_DURATION = 2; // Needs to be less than duration of scene transition

    [Header("Alternative Track")]
    [SerializeField] MusicTrack alternativeMusic;
    bool altMusicPlaying = false;
    [Range(0.01f, 20)]
    [SerializeField] float crossFadeDuration = 0.8f;

    #region Setup
    private void Awake()
    {
        SingletonChecking();

        if (CheckStandardTrackInitialized())
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
        if (CheckStandardTrackInitialized())
            StandardMusicTrackSetup();

        if (alternativeMusic.musicTrack != null)
            AlternativeMusicTrackSetup();
    }

    private void StandardMusicTrackSetup()
    {
        standardMusic.audioSource = gameObject.AddComponent<AudioSource>();
        standardMusic.audioSource.clip = standardMusic.musicTrack;
        standardMusic.audioSource.loop = true;
        if (fadeIn)
            FadeIn();
        else
        {
            standardMusic.audioSource.volume = standardMusic.volume;
            standardMusic.audioSource.Play();
        }
    }

    private void AlternativeMusicTrackSetup()
    {
        alternativeMusic.audioSource = gameObject.AddComponent<AudioSource>();
        alternativeMusic.audioSource.clip = alternativeMusic.musicTrack;
        alternativeMusic.audioSource.loop = true;
    }
    #endregion

    #region Initialization Checks
    private bool CheckStandardTrackInitialized()
    {
        if (standardMusic.musicTrack != null)
            return true;
        else
        {
            Debug.LogWarning("MusicPlayer " + name + " is missing a standard music track");
            return false;
        }
    }

    private bool CheckAlternativeTrackInitialized()
    {
        if (alternativeMusic.musicTrack != null)
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
        if (!CheckStandardTrackInitialized()) return;

        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine());
    }
    IEnumerator FadeInCoroutine()
    {
        standardMusic.audioSource.volume = 0;
        standardMusic.audioSource.Play();

        while (standardMusic.audioSource.volume < standardMusic.volume)
        {
            yield return new WaitForFixedUpdate();
            standardMusic.audioSource.volume += Time.fixedDeltaTime / fadeInDuration * standardMusic.volume;
        }

        standardMusic.audioSource.volume = standardMusic.volume;
    }
    #endregion

    #region Fade Out
    public void FadeOut()
    {
        if (!altMusicPlaying && !CheckStandardTrackInitialized()) return;
        else if (altMusicPlaying && !CheckAlternativeTrackInitialized()) return;

        fadingOut = true;

        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(altMusicPlaying));

        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    IEnumerator FadeOutCoroutine(bool alternativeTrack = false)
    {
        MusicTrack fadingOutMusicTrack = (alternativeTrack) ? alternativeMusic : standardMusic;


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
        if (fadingOut) return; // Don't crossfade if in the process of fading out

        if (!altMusicPlaying)
        {
            if (!CheckAlternativeTrackInitialized()) return;

            StopAllCoroutines();
            StartCoroutine(CrossFadeCoroutine(true));
            altMusicPlaying = true;
        }
        else
        {
            if (!CheckStandardTrackInitialized()) return;

            StopAllCoroutines();
            StartCoroutine(CrossFadeCoroutine(false));
            altMusicPlaying = false;
        }
    }
    IEnumerator CrossFadeCoroutine(bool standardToAlternative)
    {
        MusicTrack startMusicTrack = (standardToAlternative) ? standardMusic : alternativeMusic;
        MusicTrack endMusicTrack = (standardToAlternative) ? alternativeMusic : standardMusic;


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