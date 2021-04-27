using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    [Header("Standard Track")]
    [SerializeField] MusicTrack standardMusic;
    [SerializeField] bool fadeIn;
    [Range(0.01f, 20)]
    [SerializeField] float fadeInDuration = 1f;
    bool fadingOut; // Used to make sure MusicPlayer doesn't crossfade when in the middle of fading out
    const float FADE_OUT_DURATION = 0.8f; // Needs to be less than duration of scene transition

    [Header("Alternative Track")]
    [SerializeField] MusicTrack alternativeMusic;
    bool altMusicPlaying = false;
    [Range(0.01f, 20)]
    [SerializeField] float crossFadeDuration = 0.8f;

    [Header("Audio Mixer")]
    [SerializeField] AudioMixerGroup audioMixerGroup;

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
        if (audioMixerGroup)
            standardMusic.audioSource.outputAudioMixerGroup = audioMixerGroup;

        if (fadeIn)
            FadeIn();
        else
        {
            standardMusic.audioSource.volume = standardMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
            standardMusic.audioSource.Play();
        }
    }

    private void AlternativeMusicTrackSetup()
    {
        alternativeMusic.audioSource = gameObject.AddComponent<AudioSource>();
        alternativeMusic.audioSource.clip = alternativeMusic.musicTrack;
        alternativeMusic.audioSource.loop = true;
        if (audioMixerGroup)
            alternativeMusic.audioSource.outputAudioMixerGroup = audioMixerGroup;
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
    public void FadeIn()
    {
        if (!CheckStandardTrackInitialized()) return; // Don't fade in if no standard track

        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine());
    }
    IEnumerator FadeInCoroutine()
    {
        standardMusic.audioSource.volume = 0;
        standardMusic.audioSource.Play();

        while (standardMusic.audioSource.volume < standardMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            standardMusic.audioSource.volume +=
                0.05f * standardMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume / fadeInDuration;
        }

        standardMusic.audioSource.volume = standardMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
    }
    #endregion

    #region Fade Out
    public void FadeOut()
    {
        if (!altMusicPlaying && !CheckStandardTrackInitialized()) return; // Don't fade out if no standard track
        else if (altMusicPlaying && !CheckAlternativeTrackInitialized()) return; // Don't fade out if no alt track

        fadingOut = true;

        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(altMusicPlaying));

        // Allows music player to be destroyed and not carry over to the next scene
        transform.parent = null;
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    IEnumerator FadeOutCoroutine(bool alternativeTrack = false)
    {
        MusicTrack fadingOutMusicTrack = (alternativeTrack) ? alternativeMusic : standardMusic;


        while (fadingOutMusicTrack.audioSource.volume > 0)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            fadingOutMusicTrack.audioSource.volume -=
                0.05f * standardMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume / FADE_OUT_DURATION;
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
            if (!CheckAlternativeTrackInitialized()) return; // Don't crossfade to alt track if no alt track

            StopAllCoroutines();
            StartCoroutine(CrossFadeCoroutine(true));
            altMusicPlaying = true;
        }
        else
        {
            if (!CheckStandardTrackInitialized()) return; // Don't crossfade to standard track if no standard track

            StopAllCoroutines();
            StartCoroutine(CrossFadeCoroutine(false));
            altMusicPlaying = false;
        }
    }
    IEnumerator CrossFadeCoroutine(bool standardToAlternative)
    {
        MusicTrack startMusicTrack = (standardToAlternative) ? standardMusic : alternativeMusic;
        MusicTrack endMusicTrack = (standardToAlternative) ? alternativeMusic : standardMusic;

        endMusicTrack.audioSource.volume = 0;
        endMusicTrack.audioSource.Play();

        while (startMusicTrack.audioSource.volume > 0 || endMusicTrack.audioSource.volume < endMusicTrack.volume)
        {
            yield return new WaitForFixedUpdate();
            startMusicTrack.audioSource.volume -=
                Time.fixedDeltaTime / crossFadeDuration * startMusicTrack.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
            endMusicTrack.audioSource.volume +=
                Time.fixedDeltaTime / crossFadeDuration * endMusicTrack.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
        }

        endMusicTrack.audioSource.volume = endMusicTrack.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
        startMusicTrack.audioSource.Stop();
    }
    #endregion

    #region Volume Sliders
    public void RefreshMusicVolume()
    {
        StopAllCoroutines(); // Stopping any fading

        // If crossfading
        if (standardMusic.audioSource.isPlaying && alternativeMusic.audioSource.isPlaying)
        {
            if (altMusicPlaying)
            {
                standardMusic.audioSource.Stop();
                alternativeMusic.audioSource.volume = alternativeMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
            }
            else
            {
                alternativeMusic.audioSource.Stop();
                standardMusic.audioSource.volume = standardMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
            }
        }
        // If not crossfading
        else
        {
            standardMusic.audioSource.volume = standardMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
            alternativeMusic.audioSource.volume = alternativeMusic.volume * GlobalAudioSliders.masterVolume * GlobalAudioSliders.musicVolume;
        }
    }
    #endregion
}