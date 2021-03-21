using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovementVFX : MonoBehaviour
{
    [Header("Is Speeding?")]
    [SerializeField] CinemachineDollyCart cinemachineDolly;
    [SerializeField] float speedingThreshold = 10;
    const float DELAY_BETWEEN_REFRESHING_SPEEDING_VARIABLE = 0.1f;

    [Header("Speeding Status (View Only)")]
    public bool speeding = false;
    [SerializeField] float curSpeed;


    [Header("Wispy Particles")]
    [SerializeField] ParticleSystem wispyParticles;

    [Header("Speed Lines")]
    [SerializeField] Image speedLinesRenderer;
    [SerializeField] Sprite[] speedLinesFrames;
    int curSpeedLinesFrame;
    [Range(0.01f, 2)]
    [SerializeField] float delayBetweenSpeedLineFrames = 0.2f;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (wispyParticles)
        {
            var particlesMain = wispyParticles.main;
            particlesMain.playOnAwake = false;
            particlesMain.loop = true;
        }
    }
#endif

    private void Start()
    {
        if (cinemachineDolly == null)
            Debug.Log("No cinemachine dolly cart attached to " + name);
        else
            StartCoroutine(RefreshingSpeedingStatus());

        if (wispyParticles == null)
            Debug.Log("No wispy particles attached to " + name);
        else
            StartCoroutine(PlayingAndStoppingWispyParticles());

        if (speedLinesFrames.Length == 0 || speedLinesRenderer == null)
            Debug.Log("No speed lines attached to " + name);
        else
            StartCoroutine(SpeedLinesAnimation());
    }

    IEnumerator RefreshingSpeedingStatus()
    {
        while (true)
        {
            yield return new WaitForSeconds(DELAY_BETWEEN_REFRESHING_SPEEDING_VARIABLE);

            curSpeed = cinemachineDolly.m_Speed;
            speeding = curSpeed >= speedingThreshold;
        }
    }

    IEnumerator PlayingAndStoppingWispyParticles()
    {
        while (true)
        {
            if (speeding && !wispyParticles.isPlaying)
                wispyParticles.Play();
            else if (!speeding && wispyParticles.isPlaying)
                wispyParticles.Stop();

            yield return null;
        }
    }

    IEnumerator SpeedLinesAnimation()
    {
        while (true)
        {
            if (speeding)
            {
                if (!speedLinesRenderer.enabled)
                    speedLinesRenderer.enabled = true;

                speedLinesRenderer.sprite = speedLinesFrames[curSpeedLinesFrame];
                curSpeedLinesFrame++;
                if (curSpeedLinesFrame >= speedLinesFrames.Length)
                    curSpeedLinesFrame = 0;
            }
            else if (speedLinesRenderer.enabled)
                speedLinesRenderer.enabled = false;

            yield return new WaitForSeconds(delayBetweenSpeedLineFrames);
        }
    }
}
