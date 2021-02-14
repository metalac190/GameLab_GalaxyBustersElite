using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Camera Settings")]
    [SerializeField] Vector3 offset = new Vector3(0, 2, 0);
    [SerializeField] Vector2 camScreenLimits = new Vector2(5, 3);
    [Range(0, 1)]
    [SerializeField] float smoothing;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    private void LateUpdate()
    {
        CameraScreenLimits();
    }

    void FollowTarget()
    {
        Vector3 localPos = transform.localPosition;
        Vector3 targetLocalPos = target.transform.localPosition;
        transform.localPosition = Vector3.SmoothDamp(localPos, new Vector3(targetLocalPos.x + offset.x, targetLocalPos.y + offset.y, localPos.z), ref velocity, smoothing);
    }

    void CameraScreenLimits()
    {
        Vector3 localPos = transform.localPosition;
        transform.localPosition = new Vector3(Mathf.Clamp(localPos.x, -camScreenLimits.x, camScreenLimits.x),
            Mathf.Clamp(localPos.y, -camScreenLimits.y, camScreenLimits.y), localPos.z);
    }
}
