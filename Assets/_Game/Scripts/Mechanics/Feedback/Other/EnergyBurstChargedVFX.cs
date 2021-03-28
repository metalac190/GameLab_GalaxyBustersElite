using UnityEngine;

public class EnergyBurstChargedVFX : MonoBehaviour
{
    [Header("Energy Blaster Reference")]
    [SerializeField] EnergyBurst energyBurst;

    [Header("(View Only)")]
    [SerializeField] bool charged;
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
        if (playerShipMeshRenderer)
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
        charged = energyBurst.IsWeaponCharged();

        if (charged && !chargedLastFrame)
            SetCharged(true);
        else if (!charged && chargedLastFrame)
            SetCharged(false);

        chargedLastFrame = charged;
    }

    void SetCharged(bool setCharged)
    {
        if (playerShipMeshRenderer && playerShipChargedMaterial)
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
