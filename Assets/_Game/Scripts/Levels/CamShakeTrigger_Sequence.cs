using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CamShakeTrigger_Sequence : MonoBehaviour
{
    [SerializeField] private int cycle;
    [SerializeField] private float Cooldown;
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
            StartCoroutine(shakeCycle());
        }
    }

    private IEnumerator shakeCycle()
    {
        for (int i = 0; i < cycle; i++)
        {
        CameraShaker.instance.Shake(screenshakeAmount);
        yield return new WaitForSeconds(Cooldown);
        }
    }
}
