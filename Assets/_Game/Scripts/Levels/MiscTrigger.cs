using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MiscTrigger : MonoBehaviour
{
    [SerializeField] private float timeToEnableSpeedLines;
    [SerializeField] private float screenshakeAmount;
    private Collider col;
    private CameraMovementFX camFX;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        camFX = FindObjectOfType<CameraMovementFX>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (timeToEnableSpeedLines != 0) StartCoroutine(SpeedLineOverride());
            if (screenshakeAmount != 0) CameraShaker.instance.Shake(screenshakeAmount);
        }
    }

    private IEnumerator SpeedLineOverride()
	{
        camFX.speedLineOverride = true;
        yield return new WaitForSeconds(timeToEnableSpeedLines);
        camFX.speedLineOverride = false;
    }
}
