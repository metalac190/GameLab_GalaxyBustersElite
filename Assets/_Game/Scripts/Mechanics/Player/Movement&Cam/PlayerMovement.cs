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

    [SerializeField] float dodgeSpeed = 40;
    public float dodgeDuration = .5f;
    public float dodgeCooldown = 1f; //Timed after dodge ends
    public bool infiniteDodge = false;
    float dodgeDurationRemaining = 0;
    float dodgeCooldownRemaining = 0;

    [Header("Collision Settings")]
    [SerializeField] float collDuration;
    [SerializeField] Vector3 collForce;
    [SerializeField] Vector3 torqueForce;
    bool isHit;

    [Header("Boundaries")]
    [Tooltip("Limit is the size of the whole rectangle, so player can travel half of x to the left, or half of x to the right")]
    [SerializeField] Vector2 playerLimits = new Vector2(5, 3);
    public Vector2 PlayerLimits { get { return playerLimits; } }

    [Header("Inspector References")]
    [SerializeField] Transform rotateTargetTransform;
    [SerializeField] Transform shipsTransform;

    [Header("Effects")]
    public UnityEvent OnDodge;
    public UnityEvent OnDodgeEnd;
    public UnityEvent OnDodgeRefresh;
    float lastFrameX, lastFrameY;
    [Range(0.01f, 0.99f)]
    [SerializeField] float inputThresholdForMovementFX = 0.01f;
    [SerializeField] UnityEvent OnStartedMoving;
    [SerializeField] UnityEvent OnStoppedMoving;

    PlayerController pc;
    Rigidbody rb;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        rb = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (dodgeDurationRemaining > 0) //Maintain x and y values until dodge is completed
        {
            if (dodgeDurationRemaining == dodgeDuration) //On first frame of dodge, lock in velocity
            {
                x = Input.GetAxisRaw("Horizontal");
                y = Input.GetAxisRaw("Vertical");
            }
            else
            {
                x = lastFrameX;
                y = lastFrameY;
            }
            dodgeDurationRemaining -= Time.deltaTime;
            if (dodgeDurationRemaining <= 0)
            {
                DodgeEnd();
                dodgeDurationRemaining = 0;
            }
        }

        if (dodgeCooldownRemaining > 0)
        {
            dodgeCooldownRemaining -= Time.deltaTime;
            if (dodgeCooldownRemaining <= 0)
            {
                DodgeRefresh();
            }
        }

        LocalMove(x, y);
        if (!isHit)
        {
            RotateTowardsDir(x, y);
            HorizontalLean(shipsTransform, x, horizontalLean, 0.1f);
        }

        Dodge();

        InvokingStartedOrStoppedMovingEvents(x, y);
    }

    protected void LateUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        if (!isHit)
            transform.localRotation = Quaternion.Euler(Vector3.zero);
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
        transform.localPosition += new Vector3(x, y, 0) * ((dodgeDurationRemaining<=0)?moveSpeed:dodgeSpeed) * Time.deltaTime;

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
        if (Input.GetKeyDown(KeyCode.Space) && dodgeCooldownRemaining <= 0)
        {
            OnDodge.Invoke();
            dodgeDurationRemaining = dodgeDuration;
            if (!infiniteDodge)
            {
                dodgeCooldownRemaining = dodgeDuration + dodgeCooldown; //Duration of dodge is not included in cooldown value
            }
        }
    }

    void DodgeEnd()
    {
        OnDodgeEnd.Invoke();
    }

    void DodgeRefresh() //Procs when the cooldown ends, if we have any feedback of dodge being available again
    {
        OnDodgeRefresh.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        // hit terrain
        if (other.gameObject.layer == 9)
        {
            if (!isHit)
                StartCoroutine(PlayerCollision());
        }
    }

    IEnumerator PlayerCollision()
    {
        isHit = true;

        rb.AddRelativeForce(Random.Range(-collForce.x, collForce.x), 
            Random.Range(-collForce.y, collForce.y),
            Random.Range(-collForce.z, collForce.z));

        rb.AddRelativeTorque(Random.Range(-torqueForce.x, torqueForce.x), 
            Random.Range(-torqueForce.y, torqueForce.y), 
            Random.Range(-torqueForce.z, torqueForce.z));

        CameraShaker.instance.Shake(pc.CameraShakeOnHit);

        yield return new WaitForSeconds(collDuration);

        isHit = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
