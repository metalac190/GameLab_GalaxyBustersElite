using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Waypoint_GUI : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .25f);
    }
}
