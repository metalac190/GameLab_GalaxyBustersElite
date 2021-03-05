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

    // references
    Transform movementTrackerTrans;
    CinemachineDollyCart movementTrackerDollyCart;
    CinemachineDollyCart cineDollyCart;
    CinemachineSmoothPath cineSmoothPath;

    // TODO- remove this
    [Header("Tester")]
    public TextMeshProUGUI uiText;

    private void Awake()
    {
        movementTrackerTrans = GameObject.Find("Movement Tracker").transform;
        movementTrackerDollyCart = movementTrackerTrans.GetComponent<CinemachineDollyCart>();

        cineDollyCart = GameObject.Find("Camera Follower").GetComponent<CinemachineDollyCart>();
        cineSmoothPath = FindObjectOfType<CinemachineSmoothPath>();

        cineDollyCart.m_Path = cineSmoothPath;
        movementTrackerDollyCart.m_Path = cineSmoothPath;
    }

    private void Start()
    {
        InitCamRailSpeed();
    }

    private void InitCamRailSpeed()
    {
        cineDollyCart.m_Speed = waypointSpeeds[0];
        movementTrackerDollyCart.m_Speed = cineDollyCart.m_Speed;

        waypointIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Removed by Bill for Pre-Alpha
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }*/

        // TODO- prob need to find a better fix
        Vector2 movementTrackerPosXY = new Vector2(movementTrackerTrans.position.x, movementTrackerTrans.position.y);
        Vector2 movementTrackerPosYZ = new Vector2(movementTrackerTrans.position.y, movementTrackerTrans.position.z);

        Vector2 nextWaypointPosXY = new Vector2(cineSmoothPath.m_Waypoints[waypointIndex].position.x, cineSmoothPath.m_Waypoints[waypointIndex].position.y);
        Vector2 nextWaypointPosYZ = new Vector2(cineSmoothPath.m_Waypoints[waypointIndex].position.y, cineSmoothPath.m_Waypoints[waypointIndex].position.z);

        if ((Vector2.Distance(movementTrackerPosXY, nextWaypointPosXY) < 1f || Vector2.Distance(movementTrackerPosYZ, nextWaypointPosYZ) < 1f)
            && waypointIndex < waypointSpeeds.Count - 1)
        {
            SetCamRailSpeed();

            waypointIndex++;
        }
    }

    void SetCamRailSpeed()
    {
        movementTrackerDollyCart.m_Speed = waypointSpeeds[waypointIndex];
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
