using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amount_Detection : MonoBehaviour
{
    public int num_of_enemies = 0;

    [SerializeField] float speed;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (num_of_enemies == 0)
            {
                CamRailManager camRailManager = FindObjectOfType<CamRailManager>();

                camRailManager.SetCamRailSpeed(speed);
            }
        }
    }

}
