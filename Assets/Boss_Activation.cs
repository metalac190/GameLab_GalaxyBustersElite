using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss_Activation : MonoBehaviour
{
    bool activated = false;

    [SerializeField]
    UnityEvent activeBoss = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !activated)
        {
            activated = true;
            activeBoss.Invoke();
        }
    }
}
