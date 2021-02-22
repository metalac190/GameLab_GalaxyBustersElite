using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;
    [SerializeField] Camera mainCam;

    [Header("Camera Settings")]
    [SerializeField] Vector3 offset = new Vector3(0, 0, 2);
    // curve defining the relationship between player position and camera position, 0 should eval to 0 and 1 should eval to 1
    [SerializeField] AnimationCurve cameraRelation;

    PlayerMovement playerMove;
    Vector2 cameraLimits;
    Vector2 playerT;
    Vector2 cameraT;

    void Start()
    {
        playerMove = target.GetComponent<PlayerMovement>();
        cameraLimits = CalculateCameraLimits(mainCam);
    }

    private void LateUpdate()
    {
        SetCameraPosition();
    }

    Vector2 CalculateCameraLimits(Camera cam)
	{
        Vector2 limits;

        // get the dimensions of the frustum
        float distance = -1 * offset.z;

        // TODO field of view might be determined by the attached cinemachine virtual cam so maybe should use that fov value instead
        float frustumHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * cam.aspect;

        // the limits for the cameras position are the limits for the player positon but with half the frustum distances subtracted
        // this means the player won't end up outside the view
        // unlike the playerlimits this is the half length of the limits, not full, so no need to divide by 2 when using
        limits.x = playerMove.PlayerLimits.x / 2 - (frustumWidth / 2);
        limits.y = playerMove.PlayerLimits.y / 2 - (frustumHeight / 2);

        return limits;
    }

    void SetCameraPosition()
	{
        // get the t value for the player's x and y within the boundary
        // when getting the t value we work with the absolute value, getting us the distance but not direction on the x and y, and then reintroduce the direction when moving the camera to use the opposite direction
        // otherwise, we get a t value of 0.5 when the player is centered, when we want a value of 0, as the relation between positions rely on a coordinate system where (0,0) is center
        playerT = new Vector2(
            Mathf.InverseLerp(0, 0.5f * playerMove.PlayerLimits.x, Mathf.Abs(target.localPosition.x)),
            Mathf.InverseLerp(0, 0.5f * playerMove.PlayerLimits.y, Mathf.Abs(target.localPosition.y)));

        // evaluate this t value on the cameraRelation curve to get the t value to be used for the camera lerp
        cameraT = new Vector2(
            cameraRelation.Evaluate(playerT.x),
            cameraRelation.Evaluate(playerT.y));

        // put the camera in the right position with this new t value
        transform.localPosition = new Vector3(
            Mathf.Sign(target.localPosition.x) * Mathf.Lerp(0, cameraLimits.x, cameraT.x) + offset.x,
            Mathf.Sign(target.localPosition.y) * Mathf.Lerp(0, cameraLimits.y, cameraT.y) + offset.y,
            offset.z);
    }

}
