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
    [SerializeField] AudioSource laserFiringLoopSound;
    [Range(1, 100)]
    [SerializeField] float fadeInSpeed = 5;
    [Range(1, 100)]
    [SerializeField] float fadeOutSpeed = 5;
    [SerializeField] float loopingSoundDelay = 0.5f;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (laserFiringParticles)
        {
            var particlesMain = laserFiringParticles.main;
            particlesMain.playOnAwake = false;
            particlesMain.loop = true;
        }


        if (laserFiringLoopSound)
        {
            laserFiringLoopSound.loop = true;
            laserFiringLoopSound.playOnAwake = false;
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

        if (laserFiringLoopSound)
        {
            if (laserActive)
            {
                StopAllCoroutines();
                StartCoroutine(FadeInLaserSoundAfterDelay(loopingSoundDelay));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(FadeOutLaserSound());
            }
        }
    }

    IEnumerator FadeInLaserSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!laserFiringLoopSound.isPlaying) laserFiringLoopSound.Play();

        while (laserFiringLoopSound.volume < 0.99f)
        {
            laserFiringLoopSound.volume = Mathf.Clamp(laserFiringLoopSound.volume + 0.01f * fadeInSpeed, 0, 1);
            yield return new WaitForFixedUpdate();
        }

        laserFiringLoopSound.volume = 1;
    }

    IEnumerator FadeOutLaserSound()
    {
        if (!laserFiringLoopSound.isPlaying) yield break;

        while (laserFiringLoopSound.volume > 0.01f)
        {
            laserFiringLoopSound.volume = Mathf.Clamp(laserFiringLoopSound.volume - 0.01f * fadeOutSpeed, 0, 1);
            yield return new WaitForFixedUpdate();
        }

        laserFiringLoopSound.Stop();
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
