using System.Collections;
using UnityEngine;

public class AsteroidParticlesController : MonoBehaviour
{
    [SerializeField] ParticleSystem frontParticles;
    Transform frontParticlesTransform;
    [SerializeField] ParticleSystem backParticles;
    Transform backParticlesTransform;

    [SerializeField] float speedThresholdToEmit = 1;
    float speedCheckingInterval = 0.1f;

    private void Awake()
    {
        if (frontParticles)
        {
            frontParticlesTransform = frontParticles.transform;
            frontParticles.Stop();
        }
        if (backParticles)
        {
            backParticlesTransform = backParticles.transform;
            backParticles.Stop();
        }
    }

    void Start()
    {
        StartCoroutine(CheckingSpeed());
    }

    IEnumerator CheckingSpeed()
    {
        if (!frontParticles && !backParticles) yield break;

        Vector3 curPosition = transform.position;
        Vector3 lastCheckPosition = transform.position;
        float curSpeed;
        Vector3 curDirection;
        while (true)
        {
            curPosition = transform.position;

            curSpeed = (curPosition - lastCheckPosition).magnitude;

            if (curSpeed > speedThresholdToEmit)
            {
                //print(curSpeed);
                
                if (frontParticles)
                {
                    frontParticlesTransform.rotation = Quaternion.LookRotation((curPosition - lastCheckPosition).normalized);
                    if (!frontParticles.isPlaying)
                        frontParticles.Play();
                }
                if (backParticles)
                {
                    backParticlesTransform.rotation = Quaternion.LookRotation((curPosition - lastCheckPosition).normalized);
                    if (!backParticles.isPlaying)
                        backParticles.Play();
                }
            }
            else
            {
                if (frontParticles.isPlaying)
                    frontParticles.Stop();
                if (backParticles.isPlaying)
                    backParticles.Stop();
            }

            yield return new WaitForSeconds(speedCheckingInterval);

            lastCheckPosition = curPosition;
        }
    }
}
