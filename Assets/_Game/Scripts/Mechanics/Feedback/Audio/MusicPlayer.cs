using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    [SerializeField] MusicTrack music;
    [SerializeField] bool fadeIn;
    [SerializeField] float fadeInDuration = 0.8f;
    static float fadeOutDuration = 0.8f;

    #region Setup
    private void Awake()
    {
        SingletonChecking();

        MusicTrackSetup();
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
    private void MusicTrackSetup()
    {
        music.audioSource = this.gameObject.AddComponent<AudioSource>();
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
    #endregion


    #region Fade In
    void FadeIn()
    {
        music.audioSource.volume = 0;
        music.audioSource.Play();
        StartCoroutine(FadeInCoroutine());
    }
    IEnumerator FadeInCoroutine()
    {
        while (music.audioSource.volume < music.volume)
        {
            music.audioSource.volume += Time.fixedDeltaTime / fadeInDuration * music.volume;
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion


    #region Fade Out
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
        SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
    }
    IEnumerator FadeOutCoroutine()
    {
        float startingVolume = music.audioSource.volume;
        while (music.audioSource.volume > 0)
        {
            music.audioSource.volume -= Time.fixedDeltaTime / fadeOutDuration * startingVolume;
            yield return new WaitForFixedUpdate();
        }
        music.audioSource.Stop();
    }
    #endregion
}
