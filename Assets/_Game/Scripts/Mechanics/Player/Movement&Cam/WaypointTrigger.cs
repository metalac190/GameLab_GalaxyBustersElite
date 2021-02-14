using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    [SerializeField] float railSpeed;
    Tester tester;

    // Start is called before the first frame update
    void Start()
    {
        tester = FindObjectOfType<Tester>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            tester.SetRailSpeed(railSpeed);
        }
    }
}
