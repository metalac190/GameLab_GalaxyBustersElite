using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class CamRailManager : MonoBehaviour
{
    [Header("Debugger")]
    [SerializeField] float playerSpeed;
    [SerializeField] float movementTrackerSpeed;
    [SerializeField] float currentWaypointIndex;
    [SerializeField] float currentWaypointSpeed;

    [Header("Cam Rail Settings")]
    [SerializeField] int nextWaypointIndex;
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

        // debugger
        playerSpeed = waypointSpeeds[0];
        movementTrackerSpeed = waypointSpeeds[0];
        currentWaypointIndex = 0;
        currentWaypointSpeed = waypointSpeeds[0];

        nextWaypointIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementTrackerPosXY = new Vector2(movementTrackerTrans.position.x, movementTrackerTrans.position.y);
        Vector2 movementTrackerPosYZ = new Vector2(movementTrackerTrans.position.y, movementTrackerTrans.position.z);

        Vector2 nextWaypointPosXY = new Vector2(cineSmoothPath.m_Waypoints[nextWaypointIndex].position.x, cineSmoothPath.m_Waypoints[nextWaypointIndex].position.y);
        Vector2 nextWaypointPosYZ = new Vector2(cineSmoothPath.m_Waypoints[nextWaypointIndex].position.y, cineSmoothPath.m_Waypoints[nextWaypointIndex].position.z);

        if ((Vector2.Distance(movementTrackerPosXY, nextWaypointPosXY) < 10f || Vector2.Distance(movementTrackerPosYZ, nextWaypointPosYZ) < 10f)
            && nextWaypointIndex < waypointSpeeds.Count - 1)
        {
            SetCamRailSpeed(waypointSpeeds[nextWaypointIndex]);
            currentWaypointSpeed = waypointSpeeds[nextWaypointIndex];

            currentWaypointIndex = nextWaypointIndex;
            nextWaypointIndex++;
        }
    }

    public void SetCamRailSpeed(float newMS)
    {
        StopAllCoroutines();
        StartCoroutine(SetCamRailSpeedCoroutine(newMS));
    }

    // increase rail speed when player destroys an enemy
    public void IncreaseCamRailSpeed()
    {
        StopAllCoroutines();

        float newMS = movementTrackerDollyCart.m_Speed + increaseMSAmt;

        StartCoroutine(SetCamRailSpeedCoroutine(newMS));
    }

    IEnumerator SetCamRailSpeedCoroutine(float ms)
    {
        float counter = 0;

        while (counter < 1)
        {
            counter += Time.deltaTime / transitionMSDuration;
            cineDollyCart.m_Speed = Mathf.Lerp(cineDollyCart.m_Speed, ms, counter);
            movementTrackerDollyCart.m_Speed = cineDollyCart.m_Speed;

            playerSpeed = cineDollyCart.m_Speed;
            movementTrackerSpeed = movementTrackerDollyCart.m_Speed;

            yield return null;
        }
    }
}
