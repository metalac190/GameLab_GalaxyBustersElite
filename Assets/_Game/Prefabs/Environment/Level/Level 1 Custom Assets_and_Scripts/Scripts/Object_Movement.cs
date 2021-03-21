using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Movement : MonoBehaviour
{
    public float speed = 0;
    [SerializeField] Transform object_waypoint = null;

    void Update()
    {
        object_move();
        if(transform.position == object_waypoint.position)
        {
            Destroy(gameObject);

        }
    }
    private void object_move()
    {
        object_waypoint.GetComponent<Transform>();
        transform.position = Vector3.MoveTowards(transform.position, object_waypoint.position, speed * Time.deltaTime);
    }

}
