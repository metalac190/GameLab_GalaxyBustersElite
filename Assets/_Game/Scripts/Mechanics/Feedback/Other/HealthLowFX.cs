using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthLowFX : MonoBehaviour
{
    [Header("Player Health Low?")]
    [SerializeField] PlayerController playerController;
    [Range(1, 100)]
    [SerializeField] float healthLowThreshold = 30;
    [Range(1, 100)]
    [SerializeField] float healthCriticalThreshold = 15;
    const float DELAY_BETWEEN_HEALTH_CHECKS = 0.1f;

    [Header("Player Health Status (View Only)")]
    [SerializeField] float curHealth;

    [Header("Health Low Vignette")]
    [SerializeField] Image vignetteRenderer;
    [SerializeField] Sprite[] vignetteFrames;
    int curVignetteFrame;
    [Range(0.01f, 2)]
    [SerializeField] float delayBetweenVignetteFrames = 0.2f;
    [Range(1, 100)]
    [SerializeField] float vignetteFadeSpeed = 5;
    [Range(0.1f, 1.5f)]
    [SerializeField] float vignettePulseInterval = 0.5f;

    private void Start()
    {
        if (playerController == null)
            Debug.Log("No PlayerController attached to " + name);
        else
            StartCoroutine(RefreshingHealthLowStatus());

        if (vignetteFrames.Length == 0 || vignetteRenderer == null)
            Debug.Log("No health low vignette frames attached to " + name);
        else
            StartCoroutine(HealthLowAnimation());
    }

    IEnumerator RefreshingHealthLowStatus()
    {
        while (true)
        {
            curHealth = playerController.GetPlayerHealth();

            yield return new WaitForSeconds(DELAY_BETWEEN_HEALTH_CHECKS);
        }
    }

    IEnumerator HealthLowAnimation()
    {
        vignetteRenderer.enabled = true;

        float vignetteAlpha = 0;
        bool vignetteVisible = false;
        float vignetteNextPulseTime = Time.time + vignettePulseInterval;
        while (true)
        {
            if (curHealth <= healthCriticalThreshold)
            {
                if (Time.time > vignetteNextPulseTime)
                {
                    vignetteVisible = !vignetteVisible;
                    vignetteNextPulseTime = Time.time + vignettePulseInterval;
                }

                vignetteRenderer.sprite = vignetteFrames[curVignetteFrame];
                curVignetteFrame++;
                if (curVignetteFrame >= vignetteFrames.Length)
                    curVignetteFrame = 0;
            }
            else if (curHealth <= healthLowThreshold)
            {
                vignetteVisible = true;

                vignetteRenderer.sprite = vignetteFrames[curVignetteFrame];
                curVignetteFrame++;
                if (curVignetteFrame >= vignetteFrames.Length)
                    curVignetteFrame = 0;
            }
            else
            {
                vignetteVisible = false;
            }

            if (vignetteVisible)
                vignetteAlpha = Mathf.SmoothStep(vignetteAlpha, 1, 0.01f * vignetteFadeSpeed);
            else
                vignetteAlpha = Mathf.SmoothStep(vignetteAlpha, 0, 0.01f * vignetteFadeSpeed);

            vignetteRenderer.color = new Color(1, 1, 1, vignetteAlpha);

            if (vignetteVisible)
                yield return new WaitForSeconds(delayBetweenVignetteFrames);
            else
                yield return new WaitForSeconds(delayBetweenVignetteFrames / 3);
        }
    }
}
