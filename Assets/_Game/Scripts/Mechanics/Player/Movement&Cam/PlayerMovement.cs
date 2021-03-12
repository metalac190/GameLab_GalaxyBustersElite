using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float moveSpeed = 20;
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField] float rotateSpeed = 1000;
    [SerializeField] float horizontalLean = 50;

    [Header("Boundaries")]
    [Tooltip("Limit is the size of the whole rectangle, so player can travel half of x to the left, or half of x to the right")]
    [SerializeField] Vector2 playerLimits = new Vector2(5, 3);
    public Vector2 PlayerLimits { get { return playerLimits; } }

    [Header("Inspector References")]
    [SerializeField] Transform rotateTargetTransform;
    [SerializeField] Transform shipsTransform;

    [Header("Effects")]
    public UnityEvent OnDodge;
    float lastFrameX, lastFrameY;
    [Range(0.01f, 0.99f)]
    [SerializeField] float inputThresholdForMovementFX = 0.01f;
    [SerializeField] UnityEvent OnStartedMoving;
    [SerializeField] UnityEvent OnStoppedMoving;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        LocalMove(x, y);
        RotateTowardsDir(x, y);
        HorizontalLean(shipsTransform, x, horizontalLean, 0.1f);

        Dodge();

        InvokingStartedOrStoppedMovingEvents(x, y);
    }

    private void InvokingStartedOrStoppedMovingEvents(float x, float y)
    {
        bool currentlyMoving =
            Mathf.Abs(x) >= inputThresholdForMovementFX || Mathf.Abs(y) >= inputThresholdForMovementFX;
        bool wasMovingLastFrame =
            Mathf.Abs(lastFrameX) >= inputThresholdForMovementFX || Mathf.Abs(lastFrameY) >= inputThresholdForMovementFX;

        if (!wasMovingLastFrame && currentlyMoving)
            OnStartedMoving.Invoke();
        else if (wasMovingLastFrame && !currentlyMoving)
            OnStoppedMoving.Invoke();

        lastFrameX = x;
        lastFrameY = y;
    }

    void LocalMove(float x, float y)
    {
        transform.localPosition += new Vector3(x, y, 0) * moveSpeed * Time.deltaTime;

        // clamp within boundary
        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x, -0.5f * playerLimits.x, 0.5f * playerLimits.x),
            Mathf.Clamp(transform.localPosition.y, -0.5f * playerLimits.y, 0.5f * playerLimits.y),
            transform.localPosition.z);
    }

    // rotate towards an invisible aim target in front of the ship over time for better rotational feel
    void RotateTowardsDir(float x, float y)
    {
        rotateTargetTransform.parent.position = Vector3.zero;
        rotateTargetTransform.localPosition = new Vector3(x, y, 1);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotateTargetTransform.position), 
            rotateSpeed * Time.deltaTime);
    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, 
            Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDodge.Invoke();
        }
    }
}
