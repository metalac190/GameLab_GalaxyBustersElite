using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] Waypoints;
    public float moveSpeed = 3;
    public float rotateSpeed = 0.5f;
    public float scaleSpeed = 0.5f;
    public int CurrentPoint = 0;

    void FixedUpdate()
    {
        if (transform.position != Waypoints[CurrentPoint].transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[CurrentPoint].transform.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Waypoints[CurrentPoint].transform.rotation, rotateSpeed * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, Waypoints[CurrentPoint].transform.localScale, scaleSpeed * Time.deltaTime);
        }

        if (transform.position == Waypoints[CurrentPoint].transform.position)
        {
            CurrentPoint += 1;
        }
        if (CurrentPoint >= Waypoints.Length)
        {
            CurrentPoint = 0;
        }
    }
}
