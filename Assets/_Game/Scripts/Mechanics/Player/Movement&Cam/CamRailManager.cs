using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class CamRailManager : MonoBehaviour
{
    [Header("Cam Rail Settings")]
    public int waypointIndex;
    public List<float> waypointSpeeds;

    [Header("Inspector References")]
    public Transform movementTracker;
    public CinemachineDollyCart trackerDollyCart;

    public CinemachineDollyCart cineDollyCart;
    public CinemachineSmoothPath cineSmoothPath;

    // TODO- remove this
    [Header("Tester")]
    public TextMeshProUGUI uiText;

    private void Start()
    {
        InitCamRailSpeed();
    }

    private void InitCamRailSpeed()
    {
        cineDollyCart.m_Speed = waypointSpeeds[0];
        trackerDollyCart.m_Speed = cineDollyCart.m_Speed;

        waypointIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // x is offset automatically for some reason
        Vector2 movementTrackerPos = new Vector2(movementTracker.position.y, movementTracker.position.z);
        Vector2 nextWaypointPos = new Vector2(cineSmoothPath.m_Waypoints[waypointIndex].position.y, cineSmoothPath.m_Waypoints[waypointIndex].position.z);

        if (Vector2.Distance(movementTrackerPos, nextWaypointPos) < 0.1f && waypointIndex < waypointSpeeds.Count - 1)
        {
            SetCamRailSpeed();

            waypointIndex++;
        }
    }

    void SetCamRailSpeed()
    {
        trackerDollyCart.m_Speed = waypointSpeeds[waypointIndex];
        cineDollyCart.m_Speed = waypointSpeeds[waypointIndex];
    }

    // TODO- remove this
    public void InvincibleText()
    {
        StartCoroutine(InvincibleTextCoroutine());
    }

    // TODO- remove this
    IEnumerator InvincibleTextCoroutine()
    {
        if (uiText.text == "")
        {
            uiText.text = "Invincible";

            yield return new WaitForSeconds(1);

            uiText.text = "";
        }
    }
}
