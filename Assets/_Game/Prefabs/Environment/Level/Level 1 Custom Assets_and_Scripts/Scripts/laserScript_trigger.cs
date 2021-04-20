using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserScript_trigger : MonoBehaviour
{
    public float wait_duration;
    bool Dialoguetriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Dialoguetriggered == false) 
            { 
                StartCoroutine(Wait_To_Detect());
            }
        }
    }

    IEnumerator Wait_To_Detect()
    {
        yield return new WaitForSeconds(wait_duration);
        DialogueTrigger.TriggerLaserPickupDialogue();
        Dialoguetriggered = true;

    }

}
