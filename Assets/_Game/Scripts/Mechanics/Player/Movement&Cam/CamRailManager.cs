using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class CamRailManager : MonoBehaviour
{
    [Header("Cam Rail Settings")]
    [SerializeField] int waypointIndex;
    [SerializeField] List<float> waypointSpeeds;
    [SerializeField] float increaseMSAmt;
    [SerializeField] float transitionMSDuration;

    // references
    Transform movementTrackerTrans;
    CinemachineDollyCart movementTrackerDollyCart;
    CinemachineDollyCart cineDollyCart;
    CinemachineSmoothPath cineSmoothPath;

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
            StartCoroutine(SetCamRailSpeedCoroutine(waypointSpeeds[waypointIndex]));

            waypointIndex++;
        }
    }

    void SetCamRailSpeed(float newMS)
    {
        StartCoroutine(SetCamRailSpeedCoroutine(newMS));
    }

    // increase rail speed when player destroys an enemy
    public void IncreaseCamRailSpeed()
    {
        float newMS = movementTrackerDollyCart.m_Speed + increaseMSAmt;

        StartCoroutine(SetCamRailSpeedCoroutine(newMS));
    }

    IEnumerator SetCamRailSpeedCoroutine(float ms)
    {
        float counter = 0;
        float amt = Mathf.Abs(ms - cineDollyCart.m_Speed) / transitionMSDuration;
        while (counter < transitionMSDuration)
        {
            if (ms > cineDollyCart.m_Speed)
                cineDollyCart.m_Speed += amt;
            else
                cineDollyCart.m_Speed -= amt;
            movementTrackerDollyCart.m_Speed = cineDollyCart.m_Speed;

            counter += Time.deltaTime;
            yield return null;
        }
    }
}
