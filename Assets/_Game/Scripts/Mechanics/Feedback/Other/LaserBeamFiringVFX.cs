using UnityEngine;

public class LaserBeamFiringVFX : MonoBehaviour
{
    [Header("Laser Beam Reference")]
    [SerializeField] LaserBeam laserBeam;

    [Header("(View Only)")]
    [SerializeField] bool laserActive;
    bool laserActiveLastFrame;

    [Header("Firing Particles")]
    [SerializeField] ParticleSystem laserFiringParticles;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (laserFiringParticles)
        {
            var particlesMain = laserFiringParticles.main;
            particlesMain.playOnAwake = false;
            particlesMain.loop = true;
        }
    }
#endif

    private void Start()
    {
        //if (!firingParticles)
        //    Debug.Log("No laser firing particles attached to " + name);
    }

    private void Update()
    {
        laserActive = laserBeam.GetIsLaserActive();

        if (laserActive && !laserActiveLastFrame)
            RefreshIfLaserParticlesPlaying();
        else if (!laserActive && laserActiveLastFrame)
            RefreshIfLaserParticlesPlaying();

        laserActiveLastFrame = laserActive;
    }

    void RefreshIfLaserParticlesPlaying()
    {
        if (laserFiringParticles)
        {
            if (laserActive)
                laserFiringParticles.Play();
            else
                laserFiringParticles.Stop();
        }
    }
}
