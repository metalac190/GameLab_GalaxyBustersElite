using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HeatStopper : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        HeatSeeker seeker = other.GetComponent<HeatSeeker>();
        if (seeker != null)
        {
            //trying to not be too redundant
            if (seeker.isFollowing)
            {
                Debug.Log("Found Heat Seeker");
                seeker.StopFollowing();
            }
        }

    }
}
