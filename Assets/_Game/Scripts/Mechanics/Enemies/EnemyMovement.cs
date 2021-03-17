﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints = null;
    private int nextWaypoint = 0;
    private bool onTrack = true; 
    [SerializeField] float moveSpeed;

    [Header("Effects")]
    [SerializeField] UnityEvent OnFinalWaypoint;

    private void FixedUpdate()
    {
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