using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Detect : MonoBehaviour
{
    public float wait_duration;
    [SerializeField] GameObject activate_object;
    bool firstDialogue = false;
    bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Wait_To_Detect());
        }
    } 
    IEnumerator Wait_To_Detect()
    {

        yield return new WaitForSeconds(wait_duration);
        firstDialogue = true;

    }
    private void Update()
    {
        if (firstDialogue == true)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) && finished == false)
            {
                if(finished == false)
                { 
                    DialogueTrigger.TriggerMoveDialogue();
                    finished = true;
                }
                activate_object.SetActive(true);
            }

        }
    }
}
