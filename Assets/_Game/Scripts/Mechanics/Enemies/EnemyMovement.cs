using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints = null;
    private int nextWaypoint = 0;
    private bool onTrack = true; 
    [SerializeField] float moveSpeed;
    [SerializeField] bool followPath;
    [SerializeField] bool followPathInverse;
    [SerializeField] float followPathSpeed;

    [Header("Effects")]
    [SerializeField] UnityEvent OnFinalWaypoint;

    private Transform parent;
    private CinemachinePathBase path;
    private Vector3 offsetFromPath;

    private float positionAlongPath;
    private Vector3 positionAlongPathInWorldSpace;
    private Quaternion orientationAtPosition;

    private void Start()
	{
        parent = transform.parent;
        path = FindObjectOfType<CinemachinePathBase>();
        positionAlongPath = path.FromPathNativeUnits(path.FindClosestPoint(parent.position, 1, -1, 100), CinemachinePathBase.PositionUnits.Distance);
        Quaternion inverseOrientation = Quaternion.Inverse(path.EvaluateOrientationAtUnit(positionAlongPath, CinemachinePathBase.PositionUnits.Distance));
        offsetFromPath = inverseOrientation * (parent.position - (path.EvaluatePositionAtUnit(positionAlongPath, CinemachinePathBase.PositionUnits.Distance)));
	}

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (waypoints.Length >= 1)
        {
            Gizmos.DrawLine(gameObject.transform.position, waypoints[0].transform.position);
        }
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (followPath)
		{
            // on start - save position relative to closest point on dollypath
            // move along dolly path, but add saved position
            positionAlongPath += followPathSpeed * Time.fixedDeltaTime;
            positionAlongPathInWorldSpace = path.EvaluatePositionAtUnit(positionAlongPath, CinemachinePathBase.PositionUnits.Distance);
            orientationAtPosition = path.EvaluateOrientationAtUnit(positionAlongPath, CinemachinePathBase.PositionUnits.Distance);
            parent.position = positionAlongPathInWorldSpace + (orientationAtPosition * offsetFromPath);
        }
        if (followPathInverse)
        {
            // on start - save position relative to closest point on dollypath
            // move along dolly path, but add saved position
            positionAlongPath -= followPathSpeed * Time.fixedDeltaTime;
            positionAlongPathInWorldSpace = path.EvaluatePositionAtUnit(positionAlongPath, CinemachinePathBase.PositionUnits.Distance);
            orientationAtPosition = path.EvaluateOrientationAtUnit(positionAlongPath, CinemachinePathBase.PositionUnits.Distance);
            parent.position = positionAlongPathInWorldSpace + (orientationAtPosition * offsetFromPath);
        }
        if (onTrack && nextWaypoint<waypoints.Length)
        {
            Move();
            if (transform.position == waypoints[nextWaypoint].transform.position)
            {
                waypoints[nextWaypoint].GetComponent<EnemyWaypoint>().OnReach(); //Activates the OnEnter event of that specific waypoint
                nextWaypoint++;
                if (nextWaypoint >= waypoints.Length)
                {
                    EnterFinalWaypoint();
                }
            }
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[nextWaypoint].transform.position, moveSpeed * Time.deltaTime);
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void SetOnTrack(bool set) //If you need to ignore the waypoints, set to 'false' until you're ready to continue
    {
        onTrack = set;
    }

    public void SetWaypoint(int way) //Make it set a certain waypoint as the next one, so it starts moving towards it
    {
        nextWaypoint = way;
    }

    public void RestartPath() //Start the patrol path all over again
    {
        nextWaypoint = 0;
    }

    private void EnterFinalWaypoint()
    {
        OnFinalWaypoint.Invoke(); //Probably just set to delete this enemy
    }

    public void Announce(string s) //Useful for debugging (can send this method with a string when you hit a waypoint, for instance)
    {
        Debug.Log(s);
    }
}
