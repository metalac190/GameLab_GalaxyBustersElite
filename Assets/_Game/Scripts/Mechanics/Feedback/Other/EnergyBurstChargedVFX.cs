using UnityEngine;

public class EnergyBurstChargedVFX : MonoBehaviour
{
    [Header("For testing, since not hooked-up")]
    public bool charged;
    bool chargedLastFrame;

    [Header("Charged Glowing Effect")]
    [SerializeField] MeshRenderer playerShipMeshRenderer;
    Material playerShipStandardMaterial;
    [SerializeField] Material playerShipChargedMaterial;

    [Header("Charged Particles")]
    [SerializeField] ParticleSystem chargedParticles;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (chargedParticles)
        {
            var particlesMain = chargedParticles.main;
            particlesMain.playOnAwake = false;
            particlesMain.loop = true;
        }
    }
#endif

    private void Awake()
    {
        playerShipStandardMaterial = playerShipMeshRenderer.material;
    }

    private void Start()
    {
        //if (!glowingPlayerVehicle)
        //    Debug.Log("No glowing player vehicle attached to " + name);

        //if (!chargedParticles)
        //    Debug.Log("No charged particles attached to " + name);
    }

    private void Update()
    {
        // No hookup yet because energy burst not implemented
        //charged = true;

        if (charged && !chargedLastFrame)
            SetCharged(true);
        else if (!charged && chargedLastFrame)
            SetCharged(false);

        chargedLastFrame = charged;
    }

    void SetCharged(bool setCharged)
    {
        if (playerShipMeshRenderer)
            playerShipMeshRenderer.material = (setCharged) ? playerShipChargedMaterial : playerShipStandardMaterial;

        if (chargedParticles)
        {
            if (setCharged)
                chargedParticles.Play();
            else
                chargedParticles.Stop();
        }
    }
}
