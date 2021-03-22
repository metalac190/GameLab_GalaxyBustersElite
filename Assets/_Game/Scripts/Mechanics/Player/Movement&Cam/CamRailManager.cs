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

        if ((Vector2.Distance(movementTrackerPosXY, nextWaypointPosXY) < 10f || Vector2.Distance(movementTrackerPosYZ, nextWaypointPosYZ) < 10f)
            && waypointIndex < waypointSpeeds.Count - 1)
        {
            SetCamRailSpeed(waypointSpeeds[waypointIndex]);

            waypointIndex++;
        }
    }

    void SetCamRailSpeed(float newMS)
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

            yield return null;
        }
    }
}
