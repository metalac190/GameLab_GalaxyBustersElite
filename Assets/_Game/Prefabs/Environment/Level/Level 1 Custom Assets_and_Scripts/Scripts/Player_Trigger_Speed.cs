using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Trigger_Speed : MonoBehaviour
{
    [SerializeField] float speed;
    public bool destroy_self = false;
    private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                CamRailManager camRailManager = FindObjectOfType<CamRailManager>();

                camRailManager.SetCamRailSpeed(speed);

                if(destroy_self == true)
                {
                Destroy(gameObject, .5f);
                }

            }
        }
}
