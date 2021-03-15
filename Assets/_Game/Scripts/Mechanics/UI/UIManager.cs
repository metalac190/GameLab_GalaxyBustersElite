using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject HUDObject;
    public GameObject DialogueManager;

    public GameObject Mission1Grp;
    public GameObject Mission2Grp;
    public GameObject Mission3Grp;

    //Static Triggers
    /*
    static public void ActivateMission1Debrief()
    {
        if (FindObjectOfType<UIManager>())
        {
            UIManager currentManager = FindObjectOfType<UIManager>();
            if (currentManager.Mission1Grp != null)
            {
                currentManager.Mission1Grp.SetActive(true);
            }
            else
            {
                Debug.Log("Mission1grp variable not set in UIManager.");
            }
        }
        else
        {
            Debug.Log("No UIManager in scene.");
        }
    }
    static public void ActivateMission2Debrief()
    {
        if (FindObjectOfType<UIManager>())
        {
            UIManager currentManager = FindObjectOfType<UIManager>();
            if (currentManager.Mission2Grp != null)
            {
                currentManager.Mission2Grp.SetActive(true);
            }
            else
            {
                Debug.Log("Mission2grp variable not set in UIManager.");
            }
        }
        else
        {
            Debug.Log("No UIManager in scene.");
        }
    }
    static public void ActivateMission3Debrief()
    {
        if (FindObjectOfType<UIManager>())
        {
            UIManager currentManager = FindObjectOfType<UIManager>();
            if (currentManager.Mission3Grp != null)
            {
                currentManager.Mission3Grp.SetActive(true);
            }
            else
            {
                Debug.Log("Mission3grp variable not set in UIManager.");
            }
        }
        else
        {
            Debug.Log("No UIManager in scene.");
        }
    }
    static public void DeactivateMission1Debrief()
    {
        if (FindObjectOfType<UIManager>())
        {
            UIManager currentManager = FindObjectOfType<UIManager>();
            if (currentManager.Mission1Grp != null)
            {
                currentManager.Mission1Grp.SetActive(false);
            }
            else
            {
                Debug.Log("Mission1grp variable not set in UIManager.");
            }
        }
        else
        {
            Debug.Log("No UIManager in scene.");
        }
    }
    static public void DeactivateMission2Debrief()
    {
        if (FindObjectOfType<UIManager>())
        {
            UIManager currentManager = FindObjectOfType<UIManager>();
            if (currentManager.Mission2Grp != null)
            {
                currentManager.Mission2Grp.SetActive(false);
            }
            else
            {
                Debug.Log("Mission2grp variable not set in UIManager.");
            }
        }
        else
        {
            Debug.Log("No UIManager in scene.");
        }
    }
    static public void DeactivateMission3Debrief()
    {
        if (FindObjectOfType<UIManager>())
        {
            UIManager currentManager = FindObjectOfType<UIManager>();
            if (currentManager.Mission3Grp != null)
            {
                currentManager.Mission3Grp.SetActive(false);
            }
            else
            {
                Debug.Log("Mission3grp variable not set in UIManager.");
            }
        }
        else
        {
            Debug.Log("No UIManager in scene.");
        }
    }
    */
}
