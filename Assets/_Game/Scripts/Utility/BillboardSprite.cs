using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    void Update()
    {
		transform.LookAt(Camera.main.transform);   
    }
}
