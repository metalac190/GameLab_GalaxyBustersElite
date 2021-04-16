﻿using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovementFX : MonoBehaviour
{
    [Header("Is Speeding?")]
    [SerializeField] CinemachineDollyCart cinemachineDolly;
    [SerializeField] float speedingThreshold = 10;
    const float DELAY_BETWEEN_REFRESHING_SPEEDING_VARIABLE = 0.1f;

    [Header("Speeding Status (View Only)")]
    public bool speeding = false;
    [SerializeField] float curSpeed;
    public bool speedLineOverride = false;


    [Header("Wispy Particles")]
    [SerializeField] ParticleSystem wispyParticles;
    [SerializeField] Transform playerShip;

    [Header("Speed Lines")]
    [SerializeField] Image speedLinesRenderer;
    [SerializeField] Sprite[] speedLinesFrames;
    int curSpeedLinesFrame;
    [Range(0.01f, 2)]
    [SerializeField] float delayBetweenSpeedLineFrames = 0.2f;
    [SerializeField] float maxSpeedingTime = 8;

    [Header("Speed Sound")]
    [SerializeField] AudioSource speedSound;


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

        if (wispyParticles == null || speedSound == null)
            Debug.Log("No wispy particles and/or speed sound attached to " + name);
        else
            StartCoroutine(PlayingAndStoppingWispyParticlesAndSpeedSound());

        if (speedLinesFrames.Length == 0 || speedLinesRenderer == null)
            Debug.Log("No speed lines attached to " + name);
        else
            StartCoroutine(SpeedLinesAnimation());

        // Temporary fix for wispy particles following player
        if (wispyParticles != null && playerShip != null)
        {
            wispyParticles.transform.parent = playerShip;
            wispyParticles.transform.localPosition = Vector3.zero;
            wispyParticles.transform.localScale = Vector3.one;
        }
    }

    IEnumerator RefreshingSpeedingStatus()
    {
        bool canSpeedAgain = true; // If speeding timed out, can't speed again until going below and then above speeding threshold
        while (true)
        {
            yield return new WaitForSeconds(DELAY_BETWEEN_REFRESHING_SPEEDING_VARIABLE);

            curSpeed = cinemachineDolly.m_Speed;
            if (speedLineOverride)
                speeding = true;
            else if (curSpeed >= speedingThreshold && canSpeedAgain)
            {
                speeding = true;
                canSpeedAgain = false;
                StartCoroutine(StopSpeedingIfSpeedingTooLong());
            }
            else if (curSpeed < speedingThreshold)
            {
                speeding = false;
                canSpeedAgain = true;
            }
        }
    }

    IEnumerator StopSpeedingIfSpeedingTooLong()
    {
        float timeToStopSpeeding = Time.time + maxSpeedingTime;
        while (timeToStopSpeeding > Time.time && speeding)
            yield return null;
        speeding = false;
    }

    IEnumerator PlayingAndStoppingWispyParticlesAndSpeedSound()
    {
        while (true)
        {
            if (speeding && !wispyParticles.isPlaying)
            {
                wispyParticles.Play();
                speedSound.volume = 1;
                speedSound.Play();
            }
            else if (!speeding && wispyParticles.isPlaying)
            {
                wispyParticles.Stop();
                speedSound.volume -= 0.02f;
            }

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
