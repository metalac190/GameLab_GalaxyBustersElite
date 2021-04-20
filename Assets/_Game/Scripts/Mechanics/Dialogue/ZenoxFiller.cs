using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenoxFiller : MonoBehaviour
{
    public bool ZenoxFillerActive = false;
    public float delayPeriod = 0f;
    public float minWait = 12;
    public float maxWait = 22;
    public static void EnableZenoxFiller()
    {
        ZenoxFiller zFiller = FindObjectOfType<ZenoxFiller>();
        if (zFiller)
        {
            zFiller.ZenoxFillerActive = true;
            zFiller.StartCoroutine(zFiller.ZenoxFillerCoroutine());
        }
        
    }
    public static void DisableZenoxFiller()
    {
        ZenoxFiller zFiller = FindObjectOfType<ZenoxFiller>();
        if (zFiller)
        {
            zFiller.StopCoroutine(zFiller.ZenoxFillerCoroutine());
            zFiller.ZenoxFillerActive = false;
            zFiller.gameObject.SetActive(false);         
        }
    }

    IEnumerator ZenoxFillerCoroutine()
    {
        float wait = Random.Range(minWait, maxWait);
        delayPeriod = wait;
        yield return new WaitForSeconds(wait);
        DialogueTrigger.TriggerZenoxFillerDialogue();
        StartCoroutine(ZenoxFillerCoroutine());
    }

}
