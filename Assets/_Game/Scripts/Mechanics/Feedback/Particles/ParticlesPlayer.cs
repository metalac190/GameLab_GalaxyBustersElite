using UnityEngine;

public class ParticlesPlayer : MonoBehaviour
{
    public bool playOnAwake;

    public ParticleSystem particles;


    private void Awake()
    {
        if (playOnAwake)
            particles.Play();
    }

    private void Start()
    {
        if (particles == null)
            Debug.LogWarning("ParticlesPlayer attached to " + transform.parent.name + " not given any particles to play.");
    }


    [ContextMenu("Test Play")]
    public void Play() => particles.Play();


    [ContextMenu("Test Play Detach and Destroy")]
    public void PlayThenDetachAndDestroy()
    {
        transform.parent = null;
        var main = particles.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        particles.Play();
    }
}
