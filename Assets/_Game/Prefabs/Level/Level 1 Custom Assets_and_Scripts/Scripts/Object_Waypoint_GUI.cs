using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Waypoint_GUI : MonoBehaviour
{
    public Transform target;

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1.5f);

        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
