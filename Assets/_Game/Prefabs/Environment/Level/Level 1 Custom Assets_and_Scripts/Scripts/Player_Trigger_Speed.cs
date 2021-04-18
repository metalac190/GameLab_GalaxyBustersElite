using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Trigger_Speed : MonoBehaviour
{
    [SerializeField] float speed;
    private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                CamRailManager camRailManager = FindObjectOfType<CamRailManager>();

                camRailManager.SetCamRailSpeed(speed);
            }
        }
}
