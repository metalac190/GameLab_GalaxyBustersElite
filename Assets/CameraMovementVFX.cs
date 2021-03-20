using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovementVFX : MonoBehaviour
{
    public bool speeding = true;

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
        if (wispyParticles != null && wispyParticles)
        {
            var particlesMain = wispyParticles.main;
            particlesMain.playOnAwake = false;
            particlesMain.loop = true;
        }
    }
#endif

    private void Start()
    {
        if (speedLinesFrames.Length > 0 && speedLinesRenderer != null)
            StartCoroutine(SpeedLinesAnimation());
    }

    #region Play
    public void TryPlay()
    {
        if (wispyParticles == null) return;

        Play();
    }

    void Play() { wispyParticles.Play(); }
    #endregion

    #region Stop
    public void TryStop()
    {
        if (wispyParticles == null) return;

        Stop();
    }

    void Stop() { wispyParticles.Stop(); }
    #endregion

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
