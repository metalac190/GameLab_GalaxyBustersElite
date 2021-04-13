using System.Collections;
using UnityEngine;

public class LaserBeamFiringFX : MonoBehaviour
{
    [Header("Laser Beam Reference")]
    [SerializeField] LaserBeam laserBeam;

    [Header("(View Only)")]
    [SerializeField] bool laserActive;
    bool laserActiveLastFrame;

    [Header("VFX")]
    [SerializeField] ParticleSystem laserFiringParticles;

    [Header("SFX")]
    [SerializeField] AudioSource laserFiringSound;
    [Range(1, 100)]
    [SerializeField] float fadeInOutSpeed = 5;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (laserFiringParticles)
        {
            var particlesMain = laserFiringParticles.main;
            particlesMain.playOnAwake = false;
            particlesMain.loop = true;
        }

        if (laserFiringSound)
        {
            laserFiringSound.loop = true;
            laserFiringSound.playOnAwake = false;
        }
    }
#endif

    private void Update()
    {
        laserActive = laserBeam.GetIsLaserActive();

        if (laserActive && !laserActiveLastFrame)
            RefreshFXPlaying();
        else if (!laserActive && laserActiveLastFrame)
            RefreshFXPlaying();

        laserActiveLastFrame = laserActive;

        CheckPauseToStopLaserSound();
    }

    void RefreshFXPlaying()
    {
        if (laserFiringParticles)
        {
            if (laserActive && !GameManager.gm.Paused)
                laserFiringParticles.Play();
            else
                laserFiringParticles.Stop();
        }

        if (laserFiringSound)
        {
            if (laserActive)
            {
                StopAllCoroutines();
                StartCoroutine(FadeInLaserSound());
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(FadeOutLaserSound());
            }
        }
    }

    IEnumerator FadeInLaserSound()
    {
        if (!laserFiringSound.isPlaying) laserFiringSound.Play();

        while (laserFiringSound.volume < 0.99f)
        {
            laserFiringSound.volume = Mathf.Clamp(laserFiringSound.volume + 0.01f * fadeInOutSpeed, 0, 1);
            yield return new WaitForFixedUpdate();
        }

        laserFiringSound.volume = 1;
    }

    IEnumerator FadeOutLaserSound()
    {
        if (!laserFiringSound.isPlaying) yield break;

        while (laserFiringSound.volume > 0.01f)
        {
            laserFiringSound.volume = Mathf.Clamp(laserFiringSound.volume - 0.01f * fadeInOutSpeed, 0, 1);
            yield return new WaitForFixedUpdate();
        }

        laserFiringSound.Stop();
    }

    private void CheckPauseToStopLaserSound()
    {
        if (GameManager.gm.Paused && laserActive)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutLaserSound());
            StartCoroutine(RefreshFXWhenPausingFinished());
        }
    }
    IEnumerator RefreshFXWhenPausingFinished()
    {
        yield return new WaitForSeconds(0.01f);
        RefreshFXPlaying();
    }
}
